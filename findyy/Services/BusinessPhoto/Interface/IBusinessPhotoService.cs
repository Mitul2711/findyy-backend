using findyy.Model.BusinessRegister;
using findyy.Model.Response;
namespace findyy.Services.BusinessPhotoService.Interface
{
    public interface IBusinessPhotoService
    {
        Task<Response> AddPhotoAsync(BusinessPhoto photo);
        Task<Response> SetMainPhotoAsync(long businessId, long photoId);
        Task<Response> DeletePhotoAsync(long businessId, long photoId);
        Task<Response> GetPhotosByBusinessAsync(long businessId);
        Task<Response> GetMainPhotoAsync(long businessId);
    }
}
