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
using Google.Api.Gax;

namespace MCSM_Service.Implementations
{
    public class RetreatScheduleService : BaseService, IRetreatScheduleService
    {
        private readonly IRetreatScheduleRepository _retreatScheduleRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRetreatGroupRepository _retreatGroupRepository;
        private readonly IGroupScheduleRepository _groupScheduleRepository;
        public RetreatScheduleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _retreatScheduleRepository = unitOfWork.RetreatSchedule;
            _roomRepository = unitOfWork.Room;
            _retreatGroupRepository = unitOfWork.RetreatGroup;
            _groupScheduleRepository = unitOfWork.GroupSchedule;
        }

        public async Task<ListViewModel<RetreatScheduleViewModel>> GetRetreatSchedulesOfARetreat(Guid retreatId, PaginationRequestModel pagination)
        {
            var db = _retreatScheduleRepository.GetAll()
                .Include(rs => rs.Retreat)
                .Include(rs => rs.RetreatLesson)
                .Include(rs => rs.RetreatLesson.Lesson);
                //.Include(rs => rs.UsedRoom);

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

        public async Task<ListViewModel<RetreatScheduleViewModel>> GetAllRetreatSchedule(PaginationRequestModel pagination)
        {
            var query = _retreatScheduleRepository.GetAll();

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreatSchedule = await paginatedQuery
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
                Data = retreatSchedule
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
                //UsedRoomId = _roomRepository.GetMany(r => r.Id == model.UsedRoomId).First().Id,
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

            //var validateScheduleRoom = await CheckOverlapScheduleRoom((Guid)schedule.UsedRoomId, schedule.LessonDate, schedule.LessonStart, schedule.LessonEnd);
            //if (validateScheduleRoom)
            //{
            //    throw new BadRequestException("Room is already in use at this period.");
            //}

            //var validateRoom = await ValidateRoomType((Guid)schedule.UsedRoomId);
            //if (validateRoom)
            //{
            //    throw new BadRequestException("The new schedule designated room is not meant for educational activities.");
            //}

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

            if (model.RetreatLessonId.HasValue)
            {
                existSchedule.RetreatLessonId = model.RetreatLessonId.Value;
            }

            //if (model.UsedRoomId.HasValue)
            //{
            //    existSchedule.UsedRoomId = model.UsedRoomId.Value;
            //}

            if (model.LessonDate.HasValue)
            {
                existSchedule.LessonDate = model.LessonDate.Value;
            }

            if (!string.IsNullOrWhiteSpace(model.LessonStart))
            {
                existSchedule.LessonStart = parsedLessonStart;
            }

            if (!string.IsNullOrWhiteSpace(model.LessonEnd))
            {
                existSchedule.LessonEnd = parsedLessonEnd;
            }

            //existSchedule.RetreatLessonId = model.RetreatLessonId;
            //existSchedule.UsedRoomId = model.UsedRoomId;
            //existSchedule.LessonDate = (DateOnly)model.LessonDate;
            //existSchedule.LessonStart = parsedLessonStart;
            //existSchedule.LessonEnd = parsedLessonEnd;

            var validateSchedule = await CheckOverlapScheduleRetreat(existSchedule.RetreatId, (DateOnly)model.LessonDate, existSchedule.LessonStart, existSchedule.LessonEnd);
            if (validateSchedule)
            {
                throw new BadRequestException("The updating schedule overlaps with an existing schedule for the same retreat.");
            }
            //var validateScheduleRoom = await CheckOverlapScheduleRoom((Guid)model.UsedRoomId, (DateOnly)model.LessonDate, existSchedule.LessonStart, existSchedule.LessonEnd);
            //if (validateScheduleRoom)
            //{
            //    throw new BadRequestException("Updating room is already in use at this period.");
            //}
            //var validateRoom = await ValidateRoomType((Guid)model.UsedRoomId);
            //if (validateRoom)
            //{
            //    throw new BadRequestException("The updating schedule designated room is not meant for educational activities.");
            //}

            _retreatScheduleRepository.Update(existSchedule);

            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetRetreatSchedule(id) : null!;
        }

        public async Task DeleteRetreatSchedule(Guid id)
        {
            var groupSchedules = await _groupScheduleRepository.GetMany(gs => gs.RetreatScheduleId == id).ToListAsync();

            if (groupSchedules.Any())
            {
                foreach (var groupSchedule in groupSchedules)
                {
                    _groupScheduleRepository.Remove(groupSchedule);
                }
                await _unitOfWork.SaveChanges();
            }

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
            var schedule = await _retreatScheduleRepository.GetMany(rs => rs.RetreatId == retreatId && rs.LessonDate.Equals(lessonDate) && 
            ((lessonStart >= rs.LessonStart && lessonStart < rs.LessonEnd) 
            || (lessonEnd > rs.LessonStart && lessonEnd <= rs.LessonEnd) 
            || (lessonStart <= rs.LessonStart && lessonEnd >= rs.LessonEnd)))
            .AsNoTracking()
            .AnyAsync();

            return schedule;
        }

        //public async Task<bool> CheckOverlapScheduleRoom(Guid roomId, DateOnly lessonDate, TimeOnly lessonStart, TimeOnly lessonEnd)
        //{
        //    var schedule = await _retreatScheduleRepository.GetMany(rs => rs.UsedRoomId == roomId && rs.LessonDate.Equals(lessonDate)
        //    && ((lessonStart >= rs.LessonStart && lessonStart < rs.LessonEnd) || (lessonEnd > rs.LessonStart && lessonEnd <= rs.LessonEnd) || (lessonStart <= rs.LessonStart && lessonEnd >= rs.LessonEnd)))
        //    .AsNoTracking()
        //    .AnyAsync();

        //    return schedule;
        //}

        //public async Task<bool> ValidateRoomType(Guid usedRoomId)
        //{
        //    var room = await _roomRepository.GetMany(r => r.Id == usedRoomId && (r.RoomType.Name == "Dining hall" && r.RoomType.Name == "Bed room"))
        //        .AsNoTracking()
        //        .AnyAsync();

        //    return room;
        //}
    }
}
