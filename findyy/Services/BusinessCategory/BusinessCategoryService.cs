using findyy.DTO.Business;
using findyy.DTO.BusinessCategoryDTO;
using findyy.Model.BusinessRegister;
using findyy.Model.Response;
using findyy.Repository.BusinessCategoryRepo.Interface;
using findyy.Repository.BusinessRegister.Interface;
using findyy.Services.BusinessRegister.Interface;

namespace findyy.Services.BusinessRegister
{
    public class BusinessCategoryService : IBusinessCategoryService
    {
        private readonly IBusinessCategoryRepository _repo;

        public BusinessCategoryService(IBusinessCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<Response> GetAllCategoriesAsync()
        {
            var data = await _repo.GetBusinessCategoriesWithCountAsync();
            return new Response
            {
                Status = true,
                Message = "Categories retrieved successfully",
                Data = data
            };
        }

        public async Task<Response> GetCategoryByIdAsync(long id)
        {
            var data = await _repo.GetCategoryByIdAsync(id);
            return data == null
                ? new Response
                {
                    Status = true,
                    Message = "Category not found",
                    Data = null
                }
                : new Response{ Status = true, Message = "Category retrieved successfully", Data = data };
        }

        public async Task<Response> CreateCategoryAsync(BusinessCategoryDto dto)
        {
            var category = new BusinessCategory
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
            var id = await _repo.CreateCategoryAsync(category);
            return new Response{ Status = true, Message = "Category created successfully", Data = null };
        }

        public async Task<Response> UpdateCategoryAsync(BusinessCategoryDto dto)
        {
            var category = new BusinessCategory
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
            await _repo.UpdateCategoryAsync(category);
            return new Response{ Status = true, Message = "Category updated successfully", Data = null };
        }

        public async Task<Response> DeleteCategoryAsync(long id)
        {
            await _repo.DeleteCategoryAsync(id);
            return new Response{ Status = true, Message = "Category deleted successfully", Data = null };
        }
    }
}
