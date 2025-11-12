using findyy.Model.Response;

namespace findyy.Services.ChatMessage.Interface
{
    public interface IChatMessageService
    {
        Task<Response> GetChatBetweenUsersAsync(Guid senderId, Guid receiverId);
        Task<Response> GetAllChatsForUserAsync(Guid senderId);

        //Task<Response> SendMessageAsync(SendMessageDto dto);
        //Task<Response> MarkMessagesAsReadAsync(Guid senderId, Guid receiverId);
    }
}
