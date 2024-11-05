using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Enums;
using MCSM_Utility.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class RoomService : BaseService, IRoomService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        private readonly IRoomRepository _roomRepository;
        public RoomService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _roomTypeRepository = unitOfWork.RoomType;
            _roomRepository = unitOfWork.Room;
        }

        public async Task<ListViewModel<RoomViewModel>> GetRooms(RoomFilterModel filter, PaginationRequestModel pagination)
        {
            var query = _roomRepository.GetAll();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(r => r.Name.Contains(filter.Name));
            }

            if (filter.Status.HasValue)
            {
                query = query.Where(r => r.Status == filter.Status.Value.ToString());
            }

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .OrderByDescending(r => r.CreateAt)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);
            var rooms = await paginatedQuery
                .ProjectTo<RoomViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<RoomViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = rooms
            };
        }

        public async Task<RoomViewModel> GetRoom(Guid id)
        {
            return await _roomRepository.GetMany(r => r.Id == id)
                .ProjectTo<RoomViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Room not found");
        }

        public async Task<RoomViewModel> CreateRoom(CreateRoomModel model)
        {
            await CheckRoomType(model.RoomTypeId);

            var roomId = Guid.NewGuid();
            var room = new Room
            {
                Id = roomId,
                RoomTypeId = model.RoomTypeId,
                Name = model.Name,
                Capacity = model.Capacity,
                Status = RoomStatus.Active.ToString()
            };
            _roomRepository.Add(room);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetRoom(roomId) : null!;
        }

        public async Task<RoomViewModel> UpdateRoom(Guid id, UpdateRoomModel model)
        {
            var existRoom = await _roomRepository.GetMany(r => r.Id == id).FirstOrDefaultAsync() ?? throw new NotFoundException("Room not found");

            existRoom.Name = model.Name ?? existRoom.Name;
            existRoom.Capacity = model.Capacity ?? existRoom.Capacity;

            if (!string.IsNullOrWhiteSpace(model.Status))
            {
                existRoom.Status = GetRoomStatus(model.Status);
            }

            _roomRepository.Update(existRoom);
            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetRoom(id) : null!;
        }

        private async Task CheckRoomType(Guid roomTypeId)
        {
            var flag = await _roomTypeRepository.GetMany(r => r.Id == roomTypeId).FirstOrDefaultAsync() ?? throw new BadRequestException("Please re-enter room type");
        }

        private string GetRoomStatus(string status)
        {
            if (status != RoomStatus.Active.ToString() && status != RoomStatus.InActive.ToString())
            {
                throw new BadRequestException("Invalid status. Please provide either 'Active' or 'InActive'.");
            }

            return status;
        }
    }
}
