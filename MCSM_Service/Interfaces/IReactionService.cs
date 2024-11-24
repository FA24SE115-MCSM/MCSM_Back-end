using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCSM_Service.Interfaces
{
    public interface IReactionService
    {
        Task<ReactionViewModel> GetReaction(Guid id);
        Task<ReactionViewModel> CreateReaction(Guid accountId, CreateReactionModel model);
        Task<bool> UpdateReaction(Guid reactId, Guid accountId);
    }
}
