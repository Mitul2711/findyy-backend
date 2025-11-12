using findyy.Model;
using findyy.Model.BusinessRegister;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace findyy.DTO.ChatMessageDTO
{
    public class ChatMessageResponseDTO
    {
        public ChatMessageModel LastMessage { get; set; }
        public List<ChatMessageModel> ChatHistory { get; set; }
    }

    public class ChatListDTO
    {
        public Guid ReceiverId { get; set; }
        public ChatMessageModel? LastMessage { get; set; }
        public BusinessSearchResultDto? Business { get; set; }
        public UserInfoDto? UserInfo { get; set; } // ✅ Add this

    }

    public class UserInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? City { get; set; } = default!;
        public string? Phone { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Role { get; set; } = default!;
    }

}
