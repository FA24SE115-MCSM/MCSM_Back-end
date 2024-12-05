﻿using AutoMapper;
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

        public async Task<ListViewModel<GroupScheduleViewModel>> GetGroupSchedules(GroupScheduleFilterModel filter, PaginationViewModel pagination)
        {
            var query = _groupScheduleRepository.GetAll();
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

        public async Task<GroupScheduleViewModel> CreateGroupSchedule(Guid accountId, CreateGroupScheduleModel model)
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
            var validateScheduleRoom = await CheckOverlapScheduleRoom((Guid)groupSchedule.UsedRoomId, groupSchedule.RetreatSchedule.LessonDate, groupSchedule.RetreatSchedule.LessonStart, groupSchedule.RetreatSchedule.LessonEnd);
            if (validateScheduleRoom)
            {
                throw new BadRequestException("Room is already in use at this period.");
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

        //public async Task<GroupScheduleViewModel> UpdateGroupSchedule(Guid id, UpdateGroupScheduleModel model)
        //{
        //    var existSchedule = await _groupScheduleRepository.GetMany(rs => rs.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Schedule not found");
        //    if (model.RetreatScheduleId.HasValue)
        //    {
        //        existSchedule.RetreatScheduleId = model.RetreatScheduleId.Value;
        //    }

        //    if (!string.IsNullOrWhiteSpace(model.RoomName))
        //    {
        //        existSchedule.UsedRoomId = parsedLessonStart;
        //    }
        //}

        public async Task<bool> CheckOverlapScheduleRoom(Guid roomId, DateOnly lessonDate, TimeOnly lessonStart, TimeOnly lessonEnd)
        {
            var schedule = await _groupScheduleRepository.GetMany(gs => gs.UsedRoomId == roomId && gs.RetreatSchedule.LessonDate.Equals(lessonDate)
            && ((lessonStart >= gs.RetreatSchedule.LessonStart && lessonStart < gs.RetreatSchedule.LessonEnd) || (lessonEnd > gs.RetreatSchedule.LessonStart && lessonEnd <= gs.RetreatSchedule.LessonEnd) || (lessonStart <= gs.RetreatSchedule.LessonStart && lessonEnd >= gs.RetreatSchedule.LessonEnd)))
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
