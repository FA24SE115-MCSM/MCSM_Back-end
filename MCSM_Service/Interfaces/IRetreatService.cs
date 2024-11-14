using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IRetreatService
    {
        Task<ListViewModel<RetreatViewModel>> GetRetreats(RetreatFilterModel filter, PaginationRequestModel pagination);
        Task<RetreatViewModel> GetRetreat(Guid id);
        Task<RetreatViewModel> CreateRetreat(Guid accountId, CreateRetreatModel model);
        Task<RetreatViewModel> UpdateRetreat(Guid id, UpdateRetreatModel model);
        //----------------------------------------
        Task<ProgressTrackingViewModel> GetTrackingProgressOfRetreat(Guid retreatId);
        Task<ListViewModel<RetreatViewModel>> GetRetreatsOfAccount(Guid profileId, RetreatFilterModel filter, PaginationRequestModel pagination);
    }
}
