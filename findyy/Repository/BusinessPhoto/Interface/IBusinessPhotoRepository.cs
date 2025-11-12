using findyy.Model.BusinessRegister;

namespace findyy.Repositories.BusinessPhotoRepository.Interface
{
    public interface IBusinessPhotoRepository
    {
        Task<long> AddAsync(BusinessPhoto photo);
        Task<bool> SetMainAsync(long businessId, long photoId);
        Task<string?> DeleteAsync(long businessId, long photoId); // returns url that was stored (so service can delete file)
        Task<IList<BusinessPhoto>> GetByBusinessAsync(long businessId);
        Task<BusinessPhoto?> GetMainAsync(long businessId);
    }
}
