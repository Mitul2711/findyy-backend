using findyy.DTO.Business;
using findyy.Model.BusinessRegister;
using findyy.Repository.BusinessRegister.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace findyy.Repository.BusinessRegister
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly AppDbContext _db;
        public BusinessRepository(AppDbContext db) => _db = db;

        // ------------------- Business -------------------
        public async Task<Business?> GetBusinessAsync(Guid id)
        {
            // ✅ Step 1: Fetch the business with its related data
            var business = await _db.Business
                .Include(b => b.Location)
                .Include(b => b.Hours)
                .Include(b => b.BusinessCategory)
                .Where(b => b.OwnerUserId == id)
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefaultAsync();

            if (business == null)
                return null;

            // ✅ Step 2: Fetch review stats for this business
            var reviewStats = await _db.BusinessReview
                .Where(r => r.BusinessId == business.Id)
                .GroupBy(r => r.BusinessId)
                .Select(g => new
                {
                    AvgRating = (decimal)g.Average(r => (decimal?)r.RatingStarCount)!,
                    ReviewCount = g.Count()
                })
                .FirstOrDefaultAsync();

            // ✅ Step 3: Assign computed values
            if (reviewStats != null)
            {
                business.AvgRating = reviewStats.AvgRating;
                business.ReviewCount = reviewStats.ReviewCount;
            }
            else
            {
                business.AvgRating = 0;
                business.ReviewCount = 0;
            }

            return business;
        }


        public async Task<List<Business>> GetAllBusinessesAsync()
        {
            // ✅ Step 1: Preload review stats (average + count)
            var reviewStats = await _db.BusinessReview
                .GroupBy(r => r.BusinessId)
                .Select(g => new
                {
                    BusinessId = g.Key,
                    AvgRating = (decimal)g.Average(r => (decimal?)r.RatingStarCount)!, // safely cast to decimal
                    ReviewCount = g.Count()
                })
                .ToDictionaryAsync(x => x.BusinessId);

            // ✅ Step 2: Load businesses with related data
            var businesses = await _db.Business
                .Include(b => b.Location)
                .Include(b => b.Owner)
                .Include(b => b.Hours)
                .Include(b => b.BusinessCategory)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            // ✅ Step 3: Merge stats into each business
            foreach (var b in businesses)
            {
                if (reviewStats.TryGetValue(b.Id, out var stats))
                {
                    b.AvgRating = stats.AvgRating;
                    b.ReviewCount = stats.ReviewCount;
                }
                else
                {
                    b.AvgRating = 0;
                    b.ReviewCount = 0;
                }
            }

            return businesses;
        }


        public async Task<long> CreateBusinessAsync(BusinessDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", "POST"),
                new SqlParameter("@OwnerUserId", dto.OwnerUserId),
                new SqlParameter("@Name", dto.Name),
                new SqlParameter("@Description", (object?)dto.Description ?? DBNull.Value),
                new SqlParameter("@Website", (object?)dto.Website ?? DBNull.Value),

                // 🔹 Replaced old @Category with @BusinessCategoryId
                new SqlParameter("@BusinessCategoryId", (object?)dto.BusinessCategoryId ?? DBNull.Value),

                new SqlParameter("@Phone", (object?)dto.Phone ?? DBNull.Value),
                new SqlParameter("@Email", (object?)dto.Email ?? DBNull.Value),
                new SqlParameter("@IsVerified", SqlDbType.Bit) { Value = false },
                new SqlParameter("@Status", 2),
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_Business @Action=@Action, @OwnerUserId=@OwnerUserId, @Name=@Name, " +
                "@Description=@Description, @Website=@Website, @BusinessCategoryId=@BusinessCategoryId, " +
                "@Phone=@Phone, @Email=@Email, @IsVerified=@IsVerified, @Status=@Status",
                parameters
            );

            var created = await GetAllBusinessesAsync();
            return created.Last().Id;
        }

        public async Task UpdateBusinessAsync(BusinessUpdateDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", "PUT"),
                new SqlParameter("@Id", dto.Id),
                new SqlParameter("@Name", dto.Name),
                new SqlParameter("@Description", (object?)dto.Description ?? DBNull.Value),
                new SqlParameter("@Website", (object?)dto.Website ?? DBNull.Value),

                // 🔹 Replaced old @Category with @BusinessCategoryId
                new SqlParameter("@BusinessCategoryId", (object?)dto.BusinessCategoryId ?? DBNull.Value),

                new SqlParameter("@Phone", (object?)dto.Phone ?? DBNull.Value),
                new SqlParameter("@Email", (object?)dto.Email ?? DBNull.Value),
                new SqlParameter("@IsVerified", dto.IsVerified),
                new SqlParameter("@Status", dto.Status),
                new SqlParameter("@AvgRating", dto.AvgRating),
                new SqlParameter("@ReviewCount", dto.ReviewCount)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_Business @Action=@Action, @Id=@Id, @Name=@Name, @Description=@Description, " +
                "@Website=@Website, @BusinessCategoryId=@BusinessCategoryId, " +
                "@Phone=@Phone, @Email=@Email, @IsVerified=@IsVerified, @Status=@Status, " +
                "@AvgRating=@AvgRating, @ReviewCount=@ReviewCount",
                parameters
            );
        }

        public async Task DeleteBusinessAsync(long id)
        {
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC sp_Business @Action = {"DELETE"}, @Id = {id}"
            );
        }

        // ------------------- Business Location -------------------
        public async Task<BusinessLocation?> GetBusinessLocationAsync(long id)
        {
            var locations = await _db.BusinessLocation
                .FromSqlInterpolated($"EXEC sp_BusinessLocation @Action = {"GET"}, @Id = {id}")
                .ToListAsync();

            return locations.FirstOrDefault();
        }

        public async Task CreateOrUpdateLocationAsync(BusinessLocationDto dto, string action)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", action),
                new SqlParameter("@Id", dto.Id),
                new SqlParameter("@BusinessId", dto.BusinessId),
                new SqlParameter("@AddressLine1", dto.AddressLine1),
                new SqlParameter("@AddressLine2", (object?)dto.AddressLine2 ?? DBNull.Value),
                new SqlParameter("@City", dto.City),
                new SqlParameter("@State", dto.State),
                new SqlParameter("@Country", dto.Country),
                new SqlParameter("@PostalCode", (object?)dto.PostalCode ?? DBNull.Value),
                new SqlParameter("@Latitude", dto.Latitude),
                new SqlParameter("@Longitude", dto.Longitude)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_BusinessLocation @Action=@Action, @Id=@Id, @BusinessId=@BusinessId, " +
                "@AddressLine1=@AddressLine1, @AddressLine2=@AddressLine2, @City=@City, " +
                "@State=@State, @Country=@Country, @PostalCode=@PostalCode, @Latitude=@Latitude, @Longitude=@Longitude",
                parameters
            );
        }

        // ------------------- Business Hours -------------------
        public async Task<List<BusinessHour>> GetBusinessHoursAsync(long businessId)
        {
            var hours = await _db.BusinessHour
                .FromSqlInterpolated($"EXEC sp_BusinessHour @Action = {"GET_BY_BUSINESS"}, @BusinessId = {businessId}")
                .ToListAsync();

            return hours;
        }

        public async Task CreateOrUpdateBusinessHourAsync(List<BusinessHourDto> dtos, string action)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(long));
            table.Columns.Add("BusinessId", typeof(long));
            table.Columns.Add("DayOfWeek", typeof(byte));
            table.Columns.Add("OpenTime", typeof(TimeSpan));
            table.Columns.Add("CloseTime", typeof(TimeSpan));
            table.Columns.Add("IsClosed", typeof(bool));

            foreach (var dto in dtos)
            {
                table.Rows.Add(dto.Id, dto.BusinessId, dto.DayOfWeek, dto.OpenTime, dto.CloseTime, dto.IsClosed);
            }

            var parameters = new[]
            {
                new SqlParameter("@BusinessHours", table)
                {
                    TypeName = "dbo.BusinessHourTableType",
                    SqlDbType = SqlDbType.Structured
                },
                new SqlParameter("@Action", action)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_BusinessHour @BusinessHours=@BusinessHours, @Action = @Action",
                parameters
            );
        }

        // ------------------- Business Verify -------------------
        public async Task ReviewBusinessAsync(long businessId, bool isVerified, byte status)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", "REVIEW"),
                new SqlParameter("@Id", businessId),
                new SqlParameter("@IsVerified", isVerified),
                new SqlParameter("@Status", status)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_Business @Action=@Action, @Id=@Id, @IsVerified=@IsVerified, @Status=@Status",
                parameters
            );
        }

        // ------------------- Business Category -------------------
        public async Task<List<BusinessCategory>> GetBusinessCategoriesAsync()
        {
            var categories = await _db.BusinessCategory
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return categories;
        }
    }
}
