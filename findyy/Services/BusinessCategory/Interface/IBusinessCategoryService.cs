using findyy.DTO.Business;
using findyy.DTO.BusinessCategoryDTO;
using findyy.Model.Response;

namespace findyy.Services.BusinessRegister.Interface
{
    public interface IBusinessCategoryService
    {
        Task<Response> GetAllCategoriesAsync();
        Task<Response> GetCategoryByIdAsync(long id);
        Task<Response> CreateCategoryAsync(BusinessCategoryDto dto);
        Task<Response> UpdateCategoryAsync(BusinessCategoryDto dto);
        Task<Response> DeleteCategoryAsync(long id);
    }
}
