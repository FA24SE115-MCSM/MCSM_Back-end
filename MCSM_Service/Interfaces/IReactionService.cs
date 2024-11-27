using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IReactionService
    {
        Task<ReactionViewModel> GetReaction(Guid id);
        Task<ReactionViewModel> CreateReaction(Guid accountId, CreateReactionModel model);
        //Task<bool> UpdateReaction(Guid reactId, Guid accountId);
    }
}
