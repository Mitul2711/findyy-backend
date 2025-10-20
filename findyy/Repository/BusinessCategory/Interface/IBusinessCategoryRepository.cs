using findyy.DTO.BusinessCategoryDTO;
using findyy.Model.BusinessRegister;

namespace findyy.Repository.BusinessCategoryRepo.Interface
{
    public interface IBusinessCategoryRepository
    {
        Task<List<BusinessCategoryCountDTO>> GetBusinessCategoriesWithCountAsync();
        Task<BusinessCategory?> GetCategoryByIdAsync(long id);
        Task<long> CreateCategoryAsync(BusinessCategory category);
        Task UpdateCategoryAsync(BusinessCategory category);
        Task DeleteCategoryAsync(long id);
    }
}
