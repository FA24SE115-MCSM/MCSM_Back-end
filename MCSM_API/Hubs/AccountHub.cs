using MCSM_API.Configurations.Middleware;
using MCSM_Data.Models.Internal;
using MCSM_Service.Interfaces;
using MCSM_Utility.Constants;
using Microsoft.AspNetCore.SignalR;

namespace MCSM_API.Hubs
{
    [Authorize(AccountRole.Admin, AccountRole.Monk, AccountRole.Nun, AccountRole.Practitioner)]

    public class AccountHub : Hub
    {
        public static Dictionary<Guid, string> ConnectedUsers = new Dictionary<Guid, string>();
        private readonly IChatService _chatService;

        public AccountHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var auth = (AuthModel?)httpContext?.Items["User"];
            var userId = auth!.Id;

            await _chatService.IsAccountOnline(userId, true);
            ConnectedUsers[userId!] = Context.ConnectionId;

            await SendUserList();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var auth = (AuthModel?)httpContext?.Items["User"];
            var userId = auth!.Id;

            await _chatService.IsAccountOnline(userId, false);

            ConnectedUsers.Remove(userId!);
            await SendUserList();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendUserList()
        {
            var users = await _chatService.GetAccounts();
            await Clients.All.SendAsync("ReceiveUserList", users);
        }
    }
}
