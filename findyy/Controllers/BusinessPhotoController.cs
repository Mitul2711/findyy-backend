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
            // 0) delete removed photos (if any)
            if (dto.RemovedPhotoIds != null && dto.RemovedPhotoIds.Count > 0)
            {
                foreach (var id in dto.RemovedPhotoIds)
                {
                    await _service.DeleteAsync(id);
                }
            }

            // 1) upload new main photo (optional)
            if (dto.MainPhoto != null && dto.MainPhoto.Length > 0)
            {
                var mainResult = await _service.AddAsync(dto.BusinessId, dto.MainPhoto, true, dto.MainCaption ?? "Main photo");
                if (!mainResult.Status)
                    return Ok(mainResult);
            }

            // 2) upload new additional photos (optional)
            if (dto.AdditionalPhotos != null && dto.AdditionalPhotos.Count > 0)
            {
                foreach (var file in dto.AdditionalPhotos)
                {
                    if (file == null || file.Length == 0) continue;
                    await _service.AddAsync(dto.BusinessId, file, false, "Additional photo");
                }
            }

            return Ok(new { status = true, message = "Photos updated successfully" });
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
