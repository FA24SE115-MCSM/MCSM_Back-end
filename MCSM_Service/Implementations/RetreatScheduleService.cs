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
        public RetreatScheduleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _retreatScheduleRepository = unitOfWork.RetreatSchedule;
            _roomRepository = unitOfWork.Room;
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
            var schedule = new RetreatSchedule
            {
                Id = scheduleId,
                RetreatId = model.RetreatId,
                GroupId = model.GroupId,
                RetreatLessonId = model.RetreatLessionId,
                //add room type condition?
                UsedRoomId = _roomRepository.GetMany(rs => rs.Name == model.RoomName).First().Id,
                LessonDate = model.LessionDate,
                LessonStart = model.LessionStart,
                LessonEnd = model.LessonEnd,
                CreateAt = DateTime.UtcNow
            };
            _retreatScheduleRepository.Add(schedule);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetRetreatSchedule(scheduleId) : null!;

        }

        public async Task<RetreatScheduleViewModel> UpdateRetreatSchedule(Guid id, UpdateRetreatScheduleModel model)
        {
            var existSchedule = await _retreatScheduleRepository.GetMany(rs => rs.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Schedule not found");

            existSchedule.RetreatId = model.RetreatId;
            existSchedule.GroupId = model.GroupId;
            existSchedule.RetreatLessonId = model.RetreatLessonId;
            existSchedule.UsedRoomId = model.UsedRoomId;
            existSchedule.LessonDate = model.LessonDate;
            existSchedule.LessonStart = model.LessonStart;
            existSchedule.LessonEnd = model.LessonEnd;

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

        //public async Task<RetreatScheduleViewModel> GetRetreatLesson(Guid id)
        //{
        //    return await _retreatScheduleRepository.GetMany(rl => rl.Id == id)
        //                                         .Include(rl => rl.Lesson)
        //                                         .Include(rl => rl.Lesson.CreatedByNavigation)
        //                                         .Include(rl => rl.Lesson.CreatedByNavigation.Profile)
        //        .ProjectTo<RetreatScheduleViewModel>(_mapper.ConfigurationProvider)
        //        .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy retreat schedule");
        //}
    }
}
