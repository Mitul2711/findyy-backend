using findyy.Services.BusinessPhotoService.Interface;
using Microsoft.AspNetCore.Mvc;
using findyy.DTO.BusinessPhoto;

namespace findyy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPhotoController : ControllerBase
    {
        private readonly IBusinessPhotoService _service;

        public BusinessPhotoController(IBusinessPhotoService service)
        {
            _service = service;
        }

        [HttpPost("bulk")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPhotos([FromForm] BusinessphotoDTO dto)
        {
            if (dto.MainPhoto == null || dto.MainPhoto.Length == 0)
                return BadRequest("Main photo is required.");

            // Remove old photos
            await _service.RemoveAllForBusinessAsync(dto.BusinessId);

            // Save main photo
            var mainResult = await _service.AddAsync(
                dto.BusinessId,
                dto.MainPhoto,
                true,
                dto.MainCaption ?? "Main photo"
            );

            if (!mainResult.Status)
                return Ok(mainResult);

            // Save additional photos
            if (dto.AdditionalPhotos != null)
            {
                foreach (var file in dto.AdditionalPhotos)
                {
                    if (file != null && file.Length > 0)
                        await _service.AddAsync(dto.BusinessId, file, false, "Additional photo");
                }
            }

            return Ok(mainResult);
        }



        // GET api/BusinessPhoto?businessId=123
        [HttpGet]
        public async Task<IActionResult> GetPhotos([FromQuery] long businessId)
        {
            var photos = await _service.GetByBusinessIdAsync(businessId);
            return Ok(photos);
        }
    }
}
