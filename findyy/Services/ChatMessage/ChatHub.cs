using findyy.Model;
using Microsoft.AspNetCore.SignalR;

namespace findyy.Services.ChatMessageService
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            try
            {
                var chat = new ChatMessageModel
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Message = message,
                    Unread = true,
                    SentAt = DateTime.UtcNow
                };

                _context.ChatMessages.Add(chat);
                await _context.SaveChangesAsync();

                await Clients.Group(receiverId.ToString())
                    .SendAsync("ReceiveMessage", senderId, message, chat.SentAt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SendMessage: {ex}");
                throw;
            }
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId!);
                Console.WriteLine($"✅ User {userId} connected to group {userId}");
            }

            await base.OnConnectedAsync();
        }
    }
}
