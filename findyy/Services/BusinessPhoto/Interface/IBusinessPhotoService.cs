using findyy.Model.BusinessRegister;
using findyy.Model.Response;
namespace findyy.Services.BusinessPhotoService.Interface
{
    public interface IBusinessPhotoService
    {
        Task<Response> AddAsync(long businessId, IFormFile file, bool isMain, string? caption);
        Task<Response> GetByBusinessIdAsync(long businessId);
        Task RemoveAllForBusinessAsync(long businessId);
    }
}
