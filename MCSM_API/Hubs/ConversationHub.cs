using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Post;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace MCSM_API.Hubs
{
    [Authorize(AccountRole.Admin, AccountRole.Monk, AccountRole.Nun, AccountRole.Practitioner)]
    public class ConversationHub : Hub
    {
        public static Dictionary<Guid, string> ConnectedUsers = new Dictionary<Guid, string>();
        public static Dictionary<string, HashSet<string>> GroupConversation = new Dictionary<string, HashSet<string>>();

        private readonly IChatService _chatService;

        public ConversationHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var auth = (AuthModel?)httpContext?.Items["User"];
            var userId = auth!.Id;

            ConnectedUsers[userId] = Context.ConnectionId;

            await _chatService.IsAccountOnline(userId, true);
            await NotifyUserListUpdate();

            var unReads = await _chatService.GetUnReadMessage(userId);
            if (unReads != null && unReads.Count > 0)
            {
                await Clients.Caller.SendAsync("UnReads", unReads);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var auth = (AuthModel?)httpContext?.Items["User"];
            var userId = auth!.Id;

            ConnectedUsers.Remove(userId);
            await _chatService.IsAccountOnline(userId, false);
            await NotifyUserListUpdate();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task NotifyUserListUpdate()
        {
            var users = await _chatService.GetAccounts();
            await Clients.All.SendAsync("ReceiveUserList", users);
        }

        public async Task LoadChatHistory(Guid senderId, Guid receiverId)
        {
            var chatHistory = await _chatService.GetConversation(senderId, receiverId);
            var groupId = chatHistory.Id.ToString();

            if (ConnectedUsers.TryGetValue(senderId, out var connectionId))
            {
                await RemoveFromOldGroup(connectionId);
                if (!GroupConversation.ContainsKey(groupId))
                {
                    GroupConversation[groupId] = new HashSet<string>();
                }
                GroupConversation[groupId].Add(connectionId);
            }

            if (ConnectedUsers.TryGetValue(senderId, out var senderConnectionId))
            {
                await Groups.AddToGroupAsync(senderConnectionId, groupId);
            }

            if (ConnectedUsers.TryGetValue(receiverId, out var receiverConnectionId))
            {
                await Groups.AddToGroupAsync(receiverConnectionId, groupId);
                //await Clients.Client(receiverConnectionId).SendAsync("ReceiveChatHistoryClient", chatHistory);
            }

            await Clients.Group(chatHistory.Id.ToString()).SendAsync("ReceiveChatHistory", chatHistory);
            //await Clients.Caller.SendAsync("ReceiveChatHistory", chatHistory);
        }

        // Hàm xử lý logic rời khỏi group cũ, nếu đang tham gia
        private async Task RemoveFromOldGroup(string connectionId)
        {
            // Duyệt qua tất cả group
            foreach (var kvp in GroupConversation)
            {
                var groupId = kvp.Key;
                var connectionSet = kvp.Value;

                if (connectionSet.Contains(connectionId))
                {
                    connectionSet.Remove(connectionId);
                    //await Groups.RemoveFromGroupAsync(connectionId, groupId);
                    break;
                }
            }
        }

        public async Task SendMessage(CreateMessageModel model)
        {
            bool isReceiverInGroup = IsUserInGroup(model.ConversationId.ToString(), model.ReceiverId);

            model.IsRead = isReceiverInGroup;
            await _chatService.CreateMessage(model);
            var chatHistory = await _chatService.GetConversation(model.ConversationId);
            await Clients.Group(chatHistory.Id.ToString()).SendAsync("ReceiveMessage", chatHistory);

        }

        public async Task DeleteMessage(Guid messageId, Guid conversationId)
        {
            var httpContext = Context.GetHttpContext();
            var auth = (AuthModel?)httpContext?.Items["User"];
            var userId = auth!.Id;

            await _chatService.DeleteMessage(messageId, userId);
            var chatHistory = await _chatService.GetConversation(conversationId);
            await Clients.Group(chatHistory.Id.ToString()).SendAsync("ReceiveMessage", chatHistory);
        }

        private bool IsUserInGroup(string groupId, Guid userId)
        {
            if (!GroupConversation.ContainsKey(groupId))
                return false;

            if (!ConnectedUsers.TryGetValue(userId, out var connId))
                return false;

            return GroupConversation[groupId].Contains(connId);
        }

    }
}
