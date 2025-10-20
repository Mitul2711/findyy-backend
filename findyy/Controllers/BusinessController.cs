using findyy.DTO.Business;
using findyy.Services.BusinessRegister.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using findyy.Model.Response;

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
        public async Task<IActionResult> GetBusiness(Guid id)
        {
            var response = await _service.GetBusinessAsync(id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBusinesses()
        {
            var response = await _service.GetAllBusinessesAsync();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBusiness([FromBody] BusinessDto dto)
        {
            var response = await _service.CreateBusinessAsync(dto);
            return Ok(response);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateBusiness(long id, [FromBody] BusinessUpdateDto dto)
        {
            dto.Id = id;
            var response = await _service.UpdateBusinessAsync(dto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusiness(long id)
        {
            var response = await _service.DeleteBusinessAsync(id);
            return Ok(response);
        }

        // ------------------- Location Endpoints -------------------
        [HttpGet("location/{id}")]
        public async Task<IActionResult> GetBusinessLocation(long id)
        {
            var response = await _service.GetBusinessLocationAsync(id);
            return Ok(response);
        }

        [HttpPost("location")]
        public async Task<IActionResult> CreateOrUpdateLocation([FromBody] BusinessLocationDto dto)
        {
            var response = await _service.CreateOrUpdateLocationAsync(dto);
            return Ok(response);
        }
            
        // ------------------- Hours Endpoints -------------------
        [HttpGet("hours/{businessId}")]
        public async Task<IActionResult> GetBusinessHours(long businessId)
        {
            var response = await _service.GetBusinessHoursAsync(businessId);
            return Ok(response);
        }

        [HttpPost("hours")]
        public async Task<IActionResult> CreateOrUpdateHours([FromBody] List<BusinessHourDto> dtos)
        {
            var response = await _service.CreateOrUpdateBusinessHourAsync(dtos);
            return Ok(response);
        }

        [HttpPost("review/{id}")]
        public async Task<IActionResult> ReviewBusiness(long id, [FromBody] ReviewBusinessDto dto)
        {
            var response = await _service.ReviewBusinessAsync(id, dto.IsVerified, dto.Status);
            return Ok(response);
        }

    }
}
