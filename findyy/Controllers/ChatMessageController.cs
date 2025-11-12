using findyy.Services.ChatMessage.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace findyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly IChatMessageService _service;

        public ChatMessageController(IChatMessageService service)
        {
            _service = service;
        }

        [HttpGet("GetChatBetweenUsers")]
        public async Task<IActionResult> GetChatBetweenUsers(Guid senderId, Guid receiverId)
        {
            var result = await _service.GetChatBetweenUsersAsync(senderId, receiverId);
            return Ok(result);
        }

        [HttpGet("GetAllChatsForUser")]
        public async Task<IActionResult> GetAllChatsForUser(Guid senderId)
        {
            var result = await _service.GetAllChatsForUserAsync(senderId);
            return Ok(result);
        }
    }
}
