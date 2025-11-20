using findyy.Model.BusinessRegister;

namespace findyy.Repositories.BusinessPhotoRepository.Interface
{
    public interface IBusinessPhotoRepository
    {
        Task AddAsync(BusinessPhoto photo);
        Task<IEnumerable<BusinessPhoto>> GetByBusinessIdAsync(long businessId);
        Task DeleteByBusinessIdAsync(long businessId);

        Task<BusinessPhoto?> GetByIdAsync(long id);
        Task DeleteByIdAsync(long id);
        Task ClearMainAsync(long businessId);
    }
}
