using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IRetreatGroupService
    {
        Task<ListViewModel<RetreatGroupViewModel>> GetRetreatGroups(RetreatGroupFilterModel filter, PaginationRequestModel pagination);
        Task DivideGroup();
        Task<RetreatGroupViewModel> AssignedMonk(CreateMonkForGroupModel model);
    }
}
