using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IPostService
    {
        Task<ListViewModel<PostViewModel>> GetPosts(PostFilterModel filter, PaginationRequestModel pagination);
        Task<PostViewModel> GetPost(Guid id);
        Task<PostViewModel> CreatePost(Guid accountId, CreatePostModel model);
        Task<PostViewModel> UpdatePost(Guid postId, UpdatePostModel model);
    }
}
