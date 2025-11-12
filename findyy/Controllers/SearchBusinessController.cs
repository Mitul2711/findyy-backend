using findyy.DTO;
using findyy.Services.BusinessRegister.Interface;
using findyy.Services.SearchBusiness.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace findyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchBusinessController : ControllerBase
    {
        private readonly ISearchBusinessService _service;

        public SearchBusinessController(ISearchBusinessService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllBusinesses(SearchBusinessDTO data)
        {
            var response = await _service.SearchBusinessesAsync(data);
            return Ok(response);
        }
    }
}
