using findyy.DTO.Auth;
using findyy.Model.Response;
using findyy.Services.Admin.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace findyy.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminNotificationController : ControllerBase
    {
        private readonly IAdminNotificationService _adminService;

        public AdminNotificationController(IAdminNotificationService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("notify-new-business")]
        public async Task<IActionResult> NotifyNewBusiness(notificationDTO dto)
        {
            var response = await _adminService.NotifyNewBusinessAsync(dto);
            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost("notify-to-business")]
        public async Task<IActionResult> NotifyTOBusiness(notificationDTO dto)
        {
            var response = await _adminService.NotifyToBusinessAsync(dto);
            if (!response.Status)
                return BadRequest(response);

            return Ok(response);
        }
    }
}
