using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface ICommentService
    {
        Task<CommentViewModel> GetComment(Guid id);
        Task<CommentViewModel> CreateComment(Guid accountId, CreateCommentModel model);
        Task<CommentViewModel> UpdateComment(Guid id, UpdateCommentModel model);
    }
}
