using findyy.DTO.ChatMessageDTO;

namespace findyy.Repository.ChatMessageRepo.Interface
{
    public interface IChatMessageRepository
    {
        Task<ChatMessageResponseDTO> GetChatBetweenUsersAsync(Guid senderId, Guid receiverId);
        Task<List<ChatListDTO>> GetAllChatsForUserAsync(Guid senderId);
    }
}
