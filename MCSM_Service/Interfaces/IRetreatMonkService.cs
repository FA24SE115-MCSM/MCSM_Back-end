using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Models.Requests.Post;

namespace MCSM_Service.Interfaces
{
    public interface IRetreatMonkService
    {
        Task<ListViewModel<RetreatMonkViewModel>> GetRetreatMonksOfARetreat(Guid retreatId, PaginationRequestModel pagination);
        Task<RetreatMonkViewModel> GetRetreatMonk(Guid id);
        Task<RetreatMonkViewModel> CreateRetreatMonk(CreateRetreatMonkModel model);
        Task DeleteRetreatMonk(Guid id);
    }
}
