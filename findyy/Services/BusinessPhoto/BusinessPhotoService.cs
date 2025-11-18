using findyy.Model.BusinessRegister;
using findyy.Model.Response;
using findyy.Repositories.BusinessPhotoRepository.Interface;
using findyy.Services.BusinessPhotoService.Interface;

namespace findyy.Services.BusinessPhotoService
{
    public class BusinessPhotoService : IBusinessPhotoService
    {
        private readonly IBusinessPhotoRepository _repo;
        private readonly IWebHostEnvironment _env;

        public BusinessPhotoService(IBusinessPhotoRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        private string GetWebRootPath()
        {
            var rootPath = _env.WebRootPath;
            if (string.IsNullOrWhiteSpace(rootPath))
            {
                rootPath = Path.Combine(_env.ContentRootPath, "wwwroot");
            }
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            return rootPath;
        }

        public async Task<Response> AddAsync(long businessId, IFormFile file, bool isMain, string? caption)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new Response
                    {
                        Status = false,
                        Message = "File is empty or not provided",
                        Data = null
                    };
                }

                var webRoot = GetWebRootPath();
                var uploadFolder = Path.Combine(webRoot, "uploads", "business", businessId.ToString());

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = $"/uploads/business/{businessId}/{fileName}";

                var photo = new BusinessPhoto
                {
                    BusinessId = businessId,
                    Url = relativePath,
                    Caption = caption,
                    IsMain = isMain,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(photo);

                return new Response
                {
                    Status = true,
                    Message = "Business photo uploaded successfully",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = false,
                    Message = $"Failed to upload photo: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<Response> GetByBusinessIdAsync(long businessId)
        {
            try
            {
                var photos = await _repo.GetByBusinessIdAsync(businessId);

                return new Response
                {
                    Status = true,
                    Message = "Business photos retrieved successfully",
                    Data = photos
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = false,
                    Message = $"Failed to retrieve photos: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task RemoveAllForBusinessAsync(long businessId)
        {
            var photos = await _repo.GetByBusinessIdAsync(businessId);
            var webRoot = GetWebRootPath();

            foreach (var photo in photos)
            {
                if (string.IsNullOrWhiteSpace(photo.Url))
                    continue;

                // photo.Url looks like "/uploads/business/{businessId}/{fileName}"
                var relativePath = photo.Url.TrimStart('/', '\\');
                var fullPath = Path.Combine(webRoot, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch
                    {
                        // optional: log, but don't stop the whole operation
                    }
                }
            }

            await _repo.DeleteByBusinessIdAsync(businessId);
        }
    }
}