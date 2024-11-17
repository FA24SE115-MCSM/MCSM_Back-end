using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IRetreatGroupService
    {
        Task<ListViewModel<RetreatGroupViewModel>> GetRetreatGroups(RetreatGroupFilterModel filter, PaginationViewModel pagination);
        Task DivideGroup();
    }
}
