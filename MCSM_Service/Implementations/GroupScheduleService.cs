using AutoMapper;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using MCSM_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCSM_Data.Repositories.Implementations;
using MCSM_Data.Models.Views;
using MCSM_Data.Models.Requests.Filters;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MCSM_Utility.Exceptions;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Requests.Get;

namespace MCSM_Service.Implementations
{
    public class GroupScheduleService : BaseService, IGroupScheduleService
    {
        private readonly IGroupScheduleRepository _groupScheduleRepository;
        private readonly IRoomRepository _roomRepository;
        public GroupScheduleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _groupScheduleRepository = unitOfWork.GroupSchedule;
            _roomRepository = unitOfWork.Room;
        }

        public async Task<ListViewModel<GroupScheduleViewModel>> GetGroupSchedules(GroupScheduleFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _groupScheduleRepository.GetAll();

            if (filter.RetreatScheduleId != null)
            {
                query = query.Where(gs => gs.RetreatScheduleId.Equals(filter.RetreatScheduleId));
            }
            if (filter.GroupId != null)
            {
                query = query.Where(gs => gs.GroupId.Equals(filter.GroupId));
            }
            if (!string.IsNullOrEmpty(filter.RoomName))
            {
                query = query.Where(gs => gs.UsedRoom.Name.Contains(filter.RoomName));
            }

            if (filter.LessionDate != null)
            {
                query = query.Where(gs => gs.RetreatSchedule.LessonDate == filter.LessionDate);
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var groupSchedules = await paginatedQuery
                .ProjectTo<GroupScheduleViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<GroupScheduleViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = groupSchedules
            };
        }
        public async Task<GroupScheduleViewModel> GetGroupSchedule(Guid id)
        {
            return await _groupScheduleRepository.GetMany(r => r.Id == id)
                .ProjectTo<GroupScheduleViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Schedule not found");
        }

        public async Task<GroupScheduleViewModel> CreateGroupSchedule(CreateGroupScheduleModel model)
        {
            var groupScheduleId = Guid.NewGuid();
            var groupSchedule = new GroupSchedule
            {
                Id = groupScheduleId,
                RetreatScheduleId = model.RetreatScheduleId,
                GroupId = model.GroupId,
                UsedRoomId = _roomRepository.GetMany(rs => rs.Name == model.RoomName).First().Id,
                CreateAt = DateTime.UtcNow
            };
            //var validateScheduleRoom = await CheckOverlapScheduleRoom((Guid)groupSchedule.UsedRoomId, groupSchedule.RetreatSchedule.LessonDate, groupSchedule.RetreatSchedule.LessonStart, groupSchedule.RetreatSchedule.LessonEnd);
            //if (validateScheduleRoom)
            //{
            //    throw new BadRequestException("Room is already in use at this period.");
            //}
            //var validateScheduleRoom = await CheckOverlapScheduleRoom((Guid)groupSchedule.UsedRoomId, groupSchedule.RetreatScheduleId);
            //if (validateScheduleRoom)
            //{
            //    throw new BadRequestException("Room is already in use at this period.");
            //}

            var validateSchedule = await CheckOverlapScheduleGroup((Guid)groupSchedule.GroupId, groupSchedule.RetreatScheduleId);
            if (validateSchedule)
            {
                throw new BadRequestException("The new schedule overlaps with another one of the group.");
            }

            var validateRoom = await ValidateRoomType((Guid)groupSchedule.UsedRoomId);
            if (validateRoom)
            {
                throw new BadRequestException("The new schedule designated room is not meant for educational activities.");
            }

            _groupScheduleRepository.Add(groupSchedule);
            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetGroupSchedule(groupScheduleId) : null!;
        }

        public async Task<GroupScheduleViewModel> UpdateGroupSchedule(Guid id, UpdateGroupScheduleModel model)
        {
            var existSchedule = await _groupScheduleRepository.GetMany(rs => rs.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Schedule not found");
            if (model.RetreatScheduleId.HasValue)
            {
                existSchedule.RetreatScheduleId = model.RetreatScheduleId.Value;
            }
            else
            {
                throw new NotFoundException("You must enter a schedule to update.");
            }

            //if (!string.IsNullOrWhiteSpace(model.RoomName))
            //{
            //    existSchedule.UsedRoomId = _roomRepository.GetMany(r => r.Name == model.RoomName).FirstOrDefault().Id;
            //}
            existSchedule.UsedRoomId = _roomRepository.GetMany(r => r.Name == model.RoomName).FirstOrDefault().Id;

            //var validateScheduleRoom = await CheckOverlapScheduleRoom((Guid)existSchedule.UsedRoomId, existSchedule.RetreatSchedule.LessonDate, existSchedule.RetreatSchedule.LessonStart, existSchedule.RetreatSchedule.LessonEnd);
            //if (validateScheduleRoom)
            //{
            //    throw new BadRequestException("Updating room is already in use at this period.");
            //}

            //var validateScheduleRoom = await CheckOverlapScheduleRoom((Guid)existSchedule.UsedRoomId, (Guid)existSchedule.RetreatScheduleId);
            //if (validateScheduleRoom)
            //{
            //    throw new BadRequestException("Updating room is already in use at this period.");
            //}

            var validateSchedule = await CheckOverlapScheduleGroup((Guid)existSchedule.GroupId, (Guid)model.RetreatScheduleId);
            if (validateSchedule)
            {
                throw new BadRequestException("The new schedule overlaps with another one of the group.");
            }

            var validateRoom = await ValidateRoomType((Guid)existSchedule.UsedRoomId);
            if (validateRoom)
            {
                throw new BadRequestException("The updating schedule designated room is not meant for educational activities.");
            }

            _groupScheduleRepository.Update(existSchedule);

            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetGroupSchedule(id) : null!;
        }

        public async Task DeleteGroupSchedule(Guid id)
        {
            var groupSchedule = await _groupScheduleRepository.GetMany(gs => gs.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy lịch");

            _groupScheduleRepository.Remove(groupSchedule);

            await _unitOfWork.SaveChanges();
        }

        public async Task<bool> CheckOverlapScheduleRoom(Guid roomId, Guid retreatScheduleId)
        {
            var schedule = await _groupScheduleRepository.GetMany(gs => gs.UsedRoomId == roomId && gs.RetreatScheduleId == retreatScheduleId).AsNoTracking().AnyAsync();

            return schedule;
        }

        //public async Task<bool> CheckRoomCapacity(Guid roomId, Guid retreatScheduleId)
        //{
        //    var groupId = await _groupScheduleRepository.GetMany(gs => gs.UsedRoomId == roomId && gs.RetreatScheduleId == retreatScheduleId);
        //}

        public async Task<bool> ValidateRoomType(Guid usedRoomId)
        {
            var room = await _roomRepository.GetMany(r => r.Id == usedRoomId && (r.RoomType.Name == "Dining hall" && r.RoomType.Name == "Bed room"))
                .AsNoTracking()
                .AnyAsync();

            return room;
        }

        public async Task<bool> CheckOverlapScheduleGroup(Guid groupId, Guid retreatScheduleId)
        {
            var schedule = await _groupScheduleRepository.GetMany(gs => gs.GroupId == groupId && gs.RetreatScheduleId == retreatScheduleId)
                .AsNoTracking()
                .AnyAsync();

            return schedule;
        }
    }
}
