using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetRoles();
    }
}
