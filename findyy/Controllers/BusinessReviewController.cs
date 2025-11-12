using findyy.DTO.Business;
using findyy.Services.BusinessRegister.Interface;
using findyy.Services.BusinessReview.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace findyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessReviewController : ControllerBase
    {
        private readonly IBusinessReviewService _service;

        public BusinessReviewController(IBusinessReviewService service)
        {
            _service = service;
        }

        [HttpGet("user/{id:guid}")]
        public async Task<IActionResult> GetAllReviewAsync(Guid? id)
        {
            var response = await _service.GetAllOrByIdReviewForUserAsync(id);
            return Ok(response);
        }

        [HttpGet("business/{id:long}")]
        public async Task<IActionResult> GetAllForBusinessReviewAsync(long? id)
        {
            var response = await _service.GetAllOrByIdReviewForBusinessAsync(id);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] BusinessReviewDTO dto)
        {
            var response = await _service.CreateReviewAsync(dto);
            return Ok(response);
        }
    }
}
