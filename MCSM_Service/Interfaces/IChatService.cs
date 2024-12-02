using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;

namespace MCSM_Service.Interfaces
{
    public interface IChatService
    {
        Task<List<AccountViewModel>> GetAccounts();
        Task IsAccountOnline(Guid accountId, bool isOnline);

        Task<ConversationViewModel> GetConversation(Guid conversationId);
        Task<ConversationViewModel> GetConversation(Guid senderId, Guid receiverId);

        Task CreateMessage(CreateMessageModel model);
        Task DeleteMessage(Guid messageId, Guid userId);
    }
}
