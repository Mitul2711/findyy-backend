using findyy.Model.BusinessRegister;
using findyy.Model.Response;
using findyy.Repositories.BusinessPhotoRepository.Interface;
using findyy.Services.BusinessPhotoService.Interface;

namespace findyy.Services.BusinessPhotoService
{
    public class BusinessPhotoService : IBusinessPhotoService
    {
        private readonly IBusinessPhotoRepository _repo;

        public BusinessPhotoService(IBusinessPhotoRepository repo)
        {
            _repo = repo;
        }

        // ✅ Add new photo
        public async Task<Response> AddPhotoAsync(BusinessPhoto photo)
        {
            try
            {
                if (photo == null || photo.BusinessId <= 0 || string.IsNullOrEmpty(photo.Url))
                {
                    return new Response
                    {
                        Status = false,
                        Message = "Invalid photo details."
                    };
                }

                var insertedId = await _repo.AddAsync(photo);
                photo.Id = insertedId;

                return new Response
                {
                    Status = true,
                    Message = "Photo added successfully.",
                    Data = photo
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = false,
                    Message = $"Error adding photo: {ex.Message}"
                };
            }
        }

        // ✅ Set a photo as the main photo
        public async Task<Response> SetMainPhotoAsync(long businessId, long photoId)
        {
            try
            {
                var result = await _repo.SetMainAsync(businessId, photoId);

                if (!result)
                {
                    return new Response
                    {
                        Status = false,
                        Message = "Unable to set main photo. It may not exist."
                    };
                }

                return new Response
                {
                    Status = true,
                    Message = "Main photo updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = false,
                    Message = $"Error setting main photo: {ex.Message}"
                };
            }
        }

        // ✅ Delete a photo
        public async Task<Response> DeletePhotoAsync(long businessId, long photoId)
        {
            try
            {
                var deletedUrl = await _repo.DeleteAsync(businessId, photoId);

                if (string.IsNullOrEmpty(deletedUrl))
                {
                    return new Response
                    {
                        Status = false,
                        Message = "Photo not found or already deleted."
                    };
                }

                return new Response
                {
                    Status = true,
                    Message = "Photo deleted successfully.",
                    Data = deletedUrl // optional: return deleted photo URL
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = false,
                    Message = $"Error deleting photo: {ex.Message}"
                };
            }
        }

        // ✅ Get all photos for a business
        public async Task<Response> GetPhotosByBusinessAsync(long businessId)
        {
            try
            {
                var photos = await _repo.GetByBusinessAsync(businessId);

                if (photos == null || photos.Count == 0)
                {
                    return new Response
                    {
                        Status = false,
                        Message = "No photos found for this business."
                    };
                }

                return new Response
                {
                    Status = true,
                    Message = "Photos retrieved successfully.",
                    Data = photos
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = false,
                    Message = $"Error fetching photos: {ex.Message}"
                };
            }
        }

        // ✅ Get main photo of a business
        public async Task<Response> GetMainPhotoAsync(long businessId)
        {
            try
            {
                var mainPhoto = await _repo.GetMainAsync(businessId);

                if (mainPhoto == null)
                {
                    return new Response
                    {
                        Status = false,
                        Message = "Main photo not found."
                    };
                }

                return new Response
                {
                    Status = true,
                    Message = "Main photo retrieved successfully.",
                    Data = mainPhoto
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    Status = false,
                    Message = $"Error fetching main photo: {ex.Message}"
                };
            }
        }
    }
}
