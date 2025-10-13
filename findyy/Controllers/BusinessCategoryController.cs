using findyy.DTO.Business;
using findyy.DTO.BusinessCategoryDTO;
using findyy.Services.BusinessRegister.Interface;
using Microsoft.AspNetCore.Mvc;

namespace findyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCategoryController : ControllerBase
    {
        private readonly IBusinessCategoryService _service;

        public BusinessCategoryController(IBusinessCategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _service.GetAllCategoriesAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var response = await _service.GetCategoryByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessCategoryDto dto)
        {
            var response = await _service.CreateCategoryAsync(dto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] BusinessCategoryDto dto)
        {
            dto.Id = id;
            var response = await _service.UpdateCategoryAsync(dto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = await _service.DeleteCategoryAsync(id);
            return Ok(response);
        }
    }
}
