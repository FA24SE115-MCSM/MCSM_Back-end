using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IToolHistoryService
    {
        Task<ListViewModel<ToolHistoryViewModel>> GetToolHistories(ToolHistoryFilterModel filter, PaginationRequestModel pagination);
        Task<ToolHistoryViewModel> GetToolHistory(Guid id);
        Task<ToolHistoryViewModel> CreateToolHistory(CreateToolHistoryModel model);
        Task<ToolHistoryViewModel> UpdateToolHistory(Guid id, UpdateToolHistoryModel model);
    }
}
