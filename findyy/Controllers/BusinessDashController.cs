using findyy.Services.BusinessDash.Interface;
using findyy.Services.BusinessRegister.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace findyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessDashController : ControllerBase
    {
        private readonly IBusinessDashService _service;

        public BusinessDashController(IBusinessDashService service)
        {
            _service = service;
        }

        [HttpGet("dashboard/{businessId}")]
        public async Task<IActionResult> GetBusinessProfileCompletion(long businessId)
        {
            var response = await _service.GetBusinessProfileCompletionAsync(businessId);
            return Ok(response);
        }

    }
}
