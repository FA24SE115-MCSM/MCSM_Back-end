using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using MCSM_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSM_Data.Models.Views;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using MCSM_Utility.Exceptions;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Put;

namespace MCSM_Service.Implementations
{
    public class RetreatScheduleService : BaseService, IRetreatScheduleService
    {
        private readonly IRetreatScheduleRepository _retreatScheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRetreatGroupRepository _retreatGroupRepository;
        public RetreatScheduleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _retreatScheduleRepository = unitOfWork.RetreatSchedule;
            _roomRepository = unitOfWork.Room;
            _retreatGroupRepository = unitOfWork.RetreatGroup;
        }

        public async Task<ListViewModel<RetreatScheduleViewModel>> GetRetreatSchedulesOfARetreat(Guid retreatId, PaginationRequestModel pagination)
        {
            var db = _retreatScheduleRepository.GetAll().Include(rs => rs.Group)
                .Include(rs => rs.Retreat)
                .Include(rs => rs.RetreatLesson)
                .Include(rs => rs.RetreatLesson.Lesson)
                .Include(rs => rs.UsedRoom);

            var query = db.Where(rs => rs.RetreatId == retreatId);
            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreatSchedules = await paginatedQuery
                .OrderBy(r => r.LessonDate)
                .ProjectTo<RetreatScheduleViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<RetreatScheduleViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = retreatSchedules
            };
        }

        public async Task<RetreatScheduleViewModel> GetRetreatSchedule(Guid id)
        {
            return await _retreatScheduleRepository.GetMany(r => r.Id == id)
                .ProjectTo<RetreatScheduleViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Schedule not found");
        }

        public async Task<RetreatScheduleViewModel> CreateRetreatSchedule(CreateRetreatScheduleModel model)
        {
            var scheduleId = Guid.NewGuid();

            if (!TimeOnly.TryParse(model.LessonStart, out var parsedLessonStart) ||
                !TimeOnly.TryParse(model.LessonEnd, out var parsedLessonEnd))
            {
                throw new BadRequestException("Invalid time format for LessonStart or LessonEnd.");
            }

            var schedule = new RetreatSchedule
            {
                Id = scheduleId,
                RetreatId = model.RetreatId,
                //GroupId = model.GroupId,
                RetreatLessonId = model.RetreatLessionId,
                //add room type condition?
                //UsedRoomId = _roomRepository.GetMany(rs => rs.Name == model.RoomName).First().Id,
                UsedRoomId = _roomRepository.GetMany(r => r.Id == model.UsedRoomId).First().Id,
                LessonDate = model.LessonDate,
                //LessonStart = model.LessonStart,
                //LessonEnd = model.LessonEnd,
                LessonStart = parsedLessonStart,
                LessonEnd = parsedLessonEnd,
                CreateAt = DateTime.UtcNow
            };

            //var validateMatching = await ValidateGroupAndRetreatIdMatch(schedule.RetreatId, schedule.GroupId);
            //if (validateMatching!)
            //{
            //    throw new BadRequestException($"Group ID {schedule.GroupId} does not belong to retreat ID {schedule.RetreatId}.");
            //}

            var validateSchedule = await CheckOverlapScheduleRetreat(schedule.RetreatId, schedule.LessonDate, schedule.LessonStart, schedule.LessonEnd);
            if (validateSchedule) 
            {
                throw new BadRequestException("The new schedule overlaps with an existing schedule for the same retreat.");
            }

            var validateScheduleRoom = await CheckOverlapScheduleRoom((Guid)schedule.UsedRoomId, schedule.LessonDate, schedule.LessonStart, schedule.LessonEnd);
            if (validateScheduleRoom)
            {
                throw new BadRequestException("Room is already in use at this period.");
            }

            var validateRoom = await ValidateRoomType((Guid)schedule.UsedRoomId);
            if (validateRoom)
            {
                throw new BadRequestException("The new schedule designated room is not meant for educational activities.");
            }
            //var check = await CheckOverlapSchedule

            _retreatScheduleRepository.Add(schedule);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetRetreatSchedule(scheduleId) : null!;
        }

        public async Task<RetreatScheduleViewModel> UpdateRetreatSchedule(Guid id, UpdateRetreatScheduleModel model)
        {
            var existSchedule = await _retreatScheduleRepository.GetMany(rs => rs.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Schedule not found");

            if (!TimeOnly.TryParse(model.LessonStart, out var parsedLessonStart) ||
                !TimeOnly.TryParse(model.LessonEnd, out var parsedLessonEnd))
            {
                throw new BadRequestException("Invalid time format for LessonStart or LessonEnd.");
            }
            existSchedule.RetreatLessonId = model.RetreatLessonId;
            existSchedule.UsedRoomId = model.UsedRoomId;
            existSchedule.LessonDate = model.LessonDate;
            //existSchedule.LessonStart = model.LessonStart;
            //existSchedule.LessonEnd = model.LessonEnd;
            existSchedule.LessonStart = parsedLessonStart;
            existSchedule.LessonEnd = parsedLessonEnd;

            _retreatScheduleRepository.Update(existSchedule);

            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetRetreatSchedule(id) : null!;
        }

        public async Task DeleteRetreatSchedule(Guid id)
        {
            var existRetreatSchedule = await _retreatScheduleRepository.GetMany(rs => rs.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy lịch học");

            _retreatScheduleRepository.Remove(existRetreatSchedule);

            await _unitOfWork.SaveChanges();
        }

        //public async Task<bool> ValidateGroupAndRetreatIdMatch(Guid retreatId, Guid groupId)
        //{
        //    var check = await _retreatGroupRepository.GetMany(g => g.Id == groupId && g.RetreatId == retreatId)
        //        .AsNoTracking()
        //        .FirstOrDefaultAsync();
        //    return check != null;
        //}

        public async Task<bool> CheckOverlapScheduleRetreat(Guid retreatId, DateOnly lessonDate, TimeOnly lessonStart, TimeOnly lessonEnd)
        {
            var schedule = await _retreatScheduleRepository.GetMany(rs => rs.RetreatId == retreatId && rs.LessonDate.Equals(lessonDate) 
            && ((lessonStart >= rs.LessonStart && lessonStart < rs.LessonEnd) || (lessonEnd > rs.LessonStart && lessonEnd <= rs.LessonEnd) || (lessonStart <= rs.LessonStart && lessonEnd >= rs.LessonEnd)))
            .AsNoTracking()
            .AnyAsync();

            return schedule;
        }

        public async Task<bool> CheckOverlapScheduleRoom(Guid roomId, DateOnly lessonDate, TimeOnly lessonStart, TimeOnly lessonEnd)
        {
            var schedule = await _retreatScheduleRepository.GetMany(rs => rs.UsedRoomId == roomId && rs.LessonDate.Equals(lessonDate)
            && ((lessonStart >= rs.LessonStart && lessonStart < rs.LessonEnd) || (lessonEnd > rs.LessonStart && lessonEnd <= rs.LessonEnd) || (lessonStart <= rs.LessonStart && lessonEnd >= rs.LessonEnd)))
            .AsNoTracking()
            .AnyAsync();

            return schedule;
        }

        public async Task<bool> ValidateRoomType(Guid usedRoomId)
        {
            var room = await _roomRepository.GetMany(r => r.Id == usedRoomId && (r.RoomType.Name == "Dining hall" && r.RoomType.Name == "Bed room"))
                .AsNoTracking()
                .AnyAsync();

            return room;
        }
    }
}
