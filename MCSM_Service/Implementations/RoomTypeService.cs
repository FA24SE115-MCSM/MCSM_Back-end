using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class RoomTypeService : BaseService, IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        public RoomTypeService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _roomTypeRepository = unitOfWork.RoomType;
        }

        public async Task<List<RoomTypeViewModel>> GetRoomTypes()
        {
            return await _roomTypeRepository.GetAll()
                .OrderBy(r => r.Name)
                .ProjectTo<RoomTypeViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
