using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _roleRepository = unitOfWork.Role;
        }

        public async Task<List<RoleViewModel>> GetRoles()
        {
            return await _roleRepository.GetAll().ProjectTo<RoleViewModel>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
