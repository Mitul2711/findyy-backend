using findyy.DTO.BusinessCategoryDTO;
using findyy.Model.BusinessRegister;
using findyy.Repository.BusinessCategoryRepo.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace findyy.Repository.BusinessRegister
{
    public class BusinessCategoryRepository : IBusinessCategoryRepository
    {
        private readonly AppDbContext _db;

        public BusinessCategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<BusinessCategoryCountDTO>> GetBusinessCategoriesWithCountAsync()
        {
            var categories = await _db.BusinessCategory
                .Where(c => c.IsActive)
                .Select(c => new BusinessCategoryCountDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    BusinessCount = c.Businesses.Count() // count related businesses
                })
                .OrderBy(c => c.Name)
                .ToListAsync();

            return categories;
        }

        public async Task<BusinessCategory?> GetCategoryByIdAsync(long id)
        {
            var list = await _db.BusinessCategory
                .FromSqlInterpolated($"EXEC sp_BusinessCategory @Action = {"GET"}, @Id = {id}")
                .ToListAsync();
            return list.FirstOrDefault();
        }

        public async Task<long> CreateCategoryAsync(BusinessCategory category)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", "POST"),
                new SqlParameter("@Name", category.Name),
                new SqlParameter("@Description", (object?)category.Description ?? DBNull.Value),
                new SqlParameter("@IsActive", category.IsActive)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_BusinessCategory @Action=@Action, @Name=@Name, @Description=@Description, @IsActive=@IsActive",
                parameters
            );

            var latest = await _db.BusinessCategory.OrderByDescending(c => c.Id).FirstOrDefaultAsync();
            return latest?.Id ?? 0;
        }

        public async Task UpdateCategoryAsync(BusinessCategory category)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", "PUT"),
                new SqlParameter("@Id", category.Id),
                new SqlParameter("@Name", category.Name),
                new SqlParameter("@Description", (object?)category.Description ?? DBNull.Value),
                new SqlParameter("@IsActive", category.IsActive)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_BusinessCategory @Action=@Action, @Id=@Id, @Name=@Name, @Description=@Description, @IsActive=@IsActive",
                parameters
            );
        }

        public async Task DeleteCategoryAsync(long id)
        {
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC sp_BusinessCategory @Action = {"DELETE"}, @Id = {id}"
            );
        }
    }
}
