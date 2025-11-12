using findyy.DTO.ChatMessageDTO;
using findyy.Model.Response;
using findyy.Repository.ChatMessageRepo.Interface;
using findyy.Services.ChatMessage.Interface;

namespace findyy.Services.ChatMessage
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _repo;

        public ChatMessageService(IChatMessageRepository repo)
        {
            _repo = repo;
        }

        // ------------------- Get Chat Between Two Users -------------------
        public async Task<Response> GetChatBetweenUsersAsync(Guid senderId, Guid receiverId)
        {
            var chatData = await _repo.GetChatBetweenUsersAsync(senderId, receiverId);

            if (chatData == null)
            {
                return new Response
                {
                    Status = false,
                    Message = "No chat found between the users",
                    Data = null
                };
            }

            return new Response
            {
                Status = true,
                Message = "Chat retrieved successfully",
                Data = chatData
            };
        }

        public async Task<Response> GetAllChatsForUserAsync(Guid senderId)
        {
            var chatList = await _repo.GetAllChatsForUserAsync(senderId);

            if (chatList == null || !chatList.Any())
            {
                return new Response
                {
                    Status = false,
                    Message = "No chats found for this user.",
                    Data = new List<ChatListDTO>()
                };
            }

            return new Response
            {
                Status = true,
                Message = "Chat list retrieved successfully.",
                Data = chatList
            };
        }

    }
}
