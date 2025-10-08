using findyy.DTO.Business;
using findyy.Services.BusinessRegister.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace findyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _service;

        public BusinessController(IBusinessService service)
        {
            _service = service;
        }

        // ------------------- Business Endpoints -------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusiness(long id)
        {
            var business = await _service.GetBusinessAsync(id);
            if (business == null) return NotFound();
            return Ok(business);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBusinesses()
        {
            var businesses = await _service.GetAllBusinessesAsync();
            return Ok(businesses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromBody] BusinessDto dto)
        {
            var id = await _service.CreateBusinessAsync(dto);
            return Ok(new { BusinessId = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBusiness(long id, [FromBody] BusinessUpdateDto dto)
        {
            dto.Id = id;
            await _service.UpdateBusinessAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(long id)
        {
            await _service.DeleteBusinessAsync(id);
            return NoContent();
        }

        // ------------------- Location Endpoints -------------------
        [HttpGet("location/{id}")]
        public async Task<IActionResult> GetBusinessLocation(long id)
        {
            var location = await _service.GetBusinessLocationAsync(id);
            if (location == null) return NotFound();
            return Ok(location);
        }

        [HttpPost("location")]
        public async Task<IActionResult> CreateOrUpdateLocation([FromBody] BusinessLocationDto dto, [FromQuery] string action = "POST")
        {
            await _service.CreateOrUpdateLocationAsync(dto, action);
            return Ok();
        }

        // ------------------- Hours Endpoints -------------------
        [HttpGet("{businessId}/hours")]
        public async Task<IActionResult> GetBusinessHours(long businessId)
        {
            var hours = await _service.GetBusinessHoursAsync(businessId);
            return Ok(hours);
        }

        [HttpPost("hours")]
        public async Task<IActionResult> CreateOrUpdateHours([FromBody] BusinessHourDto dto, [FromQuery] string action = "POST")
        {
            await _service.CreateOrUpdateBusinessHourAsync(dto, action);
            return Ok();
        }
    }
}
