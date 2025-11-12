using findyy.Model.BusinessRegister;
using findyy.Model.Response;
using findyy.Services.BusinessPhotoService.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace findyy.Controllers
{
    [ApiController]
    [Route("api/businesses/{businessId:long}/photos")]
    public class BusinessPhotosController : ControllerBase
    {
        private readonly IBusinessPhotoService _service;

        public BusinessPhotosController(IBusinessPhotoService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add a photo metadata for a business.
        /// Expected JSON body: BusinessPhoto { BusinessId, Url, Caption, IsMain (optional) }
        /// Note: BusinessId in route must match the BusinessId in body (if provided).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add(long businessId, [FromBody] BusinessPhoto photo)
        {
            if (photo == null)
                return BadRequest(new Response { Status = false, Message = "Photo payload is required." });

            // ensure route/businessId consistency
            if (photo.BusinessId == 0)
                photo.BusinessId = businessId;
            else if (photo.BusinessId != businessId)
                return BadRequest(new Response { Status = false, Message = "BusinessId in body does not match route." });

            if (string.IsNullOrWhiteSpace(photo.Url))
                return BadRequest(new Response { Status = false, Message = "Photo Url is required." });

            var res = await _service.AddPhotoAsync(photo);

            if (!res.Status)
                return BadRequest(res);

            // Return 201 Created with location to list endpoint
            return CreatedAtAction(nameof(GetAll), new { businessId = businessId }, res);
        }

        /// <summary>
        /// Get all photos for a business
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(long businessId)
        {
            var res = await _service.GetPhotosByBusinessAsync(businessId);
            if (!res.Status)
                return NotFound(res);

            return Ok(res);
        }

        /// <summary>
        /// Get the main photo for a business
        /// </summary>
        [HttpGet("main")]
        public async Task<IActionResult> GetMain(long businessId)
        {
            var res = await _service.GetMainPhotoAsync(businessId);
            if (!res.Status)
                return NotFound(res);

            return Ok(res);
        }

        /// <summary>
        /// Set a photo as the main photo for the business
        /// </summary>
        [HttpPost("{photoId:long}/set-main")]
        public async Task<IActionResult> SetMain(long businessId, long photoId)
        {
            var res = await _service.SetMainPhotoAsync(businessId, photoId);
            if (!res.Status)
                return BadRequest(res);

            return NoContent();
        }

        /// <summary>
        /// Delete a photo by id
        /// </summary>
        [HttpDelete("{photoId:long}")]
        public async Task<IActionResult> Delete(long businessId, long photoId)
        {
            var res = await _service.DeletePhotoAsync(businessId, photoId);
            if (!res.Status)
                return NotFound(res);

            return NoContent();
        }

        /// <summary>
        /// Optional: Get single photo metadata by id
        /// (This controller method uses the list endpoint and filters — kept simple so we don't add new repo/service methods.)
        /// </summary>
        [HttpGet("{photoId:long}")]
        public async Task<IActionResult> GetPhoto(long businessId, long photoId)
        {
            var listRes = await _service.GetPhotosByBusinessAsync(businessId);
            if (!listRes.Status)
                return NotFound(listRes);

            var photos = listRes.Data as System.Collections.IEnumerable;
            if (photos == null)
                return NotFound(new Response { Status = false, Message = "No photos found." });

            // cast to list
            var typedList = listRes.Data as System.Collections.Generic.IList<BusinessPhoto>;
            if (typedList != null)
            {
                var photo = System.Linq.Enumerable.FirstOrDefault(typedList, p => p.Id == photoId);
                if (photo == null)
                    return NotFound(new Response { Status = false, Message = "Photo not found." });

                return Ok(new Response { Status = true, Message = "Photo retrieved.", Data = photo });
            }

            // fallback
            return NotFound(new Response { Status = false, Message = "Photo not found." });
        }
    }
}
