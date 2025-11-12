using findyy.DTO.Business;
using findyy.Model.BusinessReview;
using findyy.Repository.BusinessReviewRepo.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace findyy.Repository.BusinessReviewRepo
{
    public class BusinessReviewRepository : IBusinessReviewRepository
    {
        private readonly AppDbContext _db;
        public BusinessReviewRepository(AppDbContext db) => _db = db;

        public async Task<List<BusinessReview>> GetAllOrByIdReviewForUserAsync(Guid? UserId = null)
        {
            if (UserId.HasValue)
            {
                var reviews = await _db.BusinessReview
                    .FromSqlInterpolated($"EXEC sp_ManageBusinessReview @Action = {"SELECTFORUSER"}, @CreatedBy = {UserId}")
                    .ToListAsync();

                return reviews;
            }
            else
            {
                var reviews = await _db.BusinessReview
                    .FromSqlInterpolated($"EXEC sp_ManageBusinessReview @Action = {"SELECTFORUSER"}")
                    .ToListAsync();

                return reviews;
            }
        }

        public async Task<List<BusinessReview>> GetAllOrByIdReviewForBusinessAsync(long? UserId = null)
        {
            if (UserId.HasValue)
            {
                var reviews = await _db.BusinessReview
                    .FromSqlInterpolated($"EXEC sp_ManageBusinessReview @Action = {"SELECTFORBUSINESS"}, @BusinessId = {UserId}")
                    .ToListAsync();

                return reviews;
            }
            else
            {
                var reviews = await _db.BusinessReview
                    .FromSqlInterpolated($"EXEC sp_ManageBusinessReview @Action = {"SELECTFORBUSINESS"}")
                    .ToListAsync();

                return reviews;
            }
        }

        public async Task<long> CreateReviewAsync(BusinessReviewDTO dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", "INSERT"),
                new SqlParameter("@BusinessId", dto.BusinessId),
                new SqlParameter("@RatingStarCount", dto.RatingStarCount),
                new SqlParameter("@Name", dto.Name ?? (object)DBNull.Value),
                new SqlParameter("@Email", dto.Email ?? (object)DBNull.Value),
                new SqlParameter("@ReviewTitle", dto.ReviewTitle ?? (object)DBNull.Value),
                new SqlParameter("@ReviewDescription", dto.ReviewDescription ?? (object)DBNull.Value),
                new SqlParameter("@CreatedBy", dto.CreatedBy),
                new SqlParameter("@UpdatedBy", dto.UpdatedBy)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_ManageBusinessReview @Action=@Action, @BusinessId=@BusinessId, @RatingStarCount=@RatingStarCount, " +
                "@Name=@Name, @Email=@Email, @ReviewTitle=@ReviewTitle, @ReviewDescription=@ReviewDescription, " +
                "@CreatedBy=@CreatedBy, @UpdatedBy=@UpdatedBy",
                parameters
            );

            var created = await GetAllOrByIdReviewForUserAsync();
            return created.Last().Id;
        }

    }
}
