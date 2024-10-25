using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IToolService
    {
        Task<ListViewModel<ToolViewModel>> GetTools(ToolFilterModel filter, PaginationRequestModel pagination);
        Task<ToolViewModel> GetTool(Guid id);
        Task<ToolViewModel> CreateTool(CreateToolModel model);
        Task<ToolViewModel> UpdateTool(Guid id, UpdateToolModel model);
    }
}
