using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Data.Models.Requests.Post;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.SignalR;

namespace MCSM_API.Hubs
{
    [Authorize(AccountRole.Admin, AccountRole.Monk, AccountRole.Nun, AccountRole.Practitioner)]
    public class ChatHub : Hub
    {
        public static Dictionary<Guid, string> ConnectedUsers = new Dictionary<Guid, string>();
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var auth = (AuthModel?)httpContext?.Items["User"];
            var userId = auth!.Id;

            // Gán UserId với ConnectionId
            ConnectedUsers[userId!] = Context.ConnectionId;

            await Clients.All.SendAsync("UserConnected", userId);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var auth = (AuthModel?)httpContext?.Items["User"];
            var userId = auth!.Id;

            ConnectedUsers.Remove(userId!);

            await Clients.All.SendAsync("UserDisconnected", userId);

            await base.OnDisconnectedAsync(exception);
        }

        // Method to fetch chat history
        public async Task LoadChatHistory(Guid senderId, Guid receiverId)
        {
            var chatHistory = await _chatService.GetConversation(senderId, receiverId);
            if (ConnectedUsers.TryGetValue(senderId, out var senderConnectionId))
            {
                await Groups.AddToGroupAsync(senderConnectionId, chatHistory.Id.ToString());
            }

            if (ConnectedUsers.TryGetValue(receiverId, out var receiverConnectionId))
            {
                await Groups.AddToGroupAsync(receiverConnectionId, chatHistory.Id.ToString());
            }

            await Clients.Caller.SendAsync("ReceiveChatHistory", chatHistory);
        }

        public async Task SendMessage(CreateMessageModel model)
        {
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
    }
}
