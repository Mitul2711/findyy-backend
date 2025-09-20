using findyy.DTO.Business;
using findyy.Model.BusinessRegister;
using findyy.Repository.BusinessRegister.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace findyy.Repository.BusinessRegister
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly AppDbContext _db;
        public BusinessRepository(AppDbContext db) => _db = db;

        // ------------------- Business -------------------
        public async Task<Business?> GetBusinessAsync(long id)
        {
            var businesses = await _db.Business
                .FromSqlInterpolated($"EXEC sp_Business @Action = {"GET"}, @Id = {id}")
                .ToListAsync();

            return businesses.FirstOrDefault();
        }

        public async Task<List<Business>> GetAllBusinessesAsync()
        {
            var businesses = await _db.Business
                .FromSqlInterpolated($"EXEC sp_Business @Action = {"GET_ALL"}")
                .ToListAsync();

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
                new SqlParameter("@Category", (object?)dto.Category ?? DBNull.Value),
                new SqlParameter("@Phone", (object?)dto.Phone ?? DBNull.Value),
                new SqlParameter("@Email", (object?)dto.Email ?? DBNull.Value),
                new SqlParameter("@IsVerified", 0),
                new SqlParameter("@Status", 2),
                new SqlParameter("@AvgRating", dto.AvgRating),
                new SqlParameter("@ReviewCount", dto.ReviewCount)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_Business @Action=@Action, @OwnerUserId=@OwnerUserId, @Name=@Name, " +
                "@Description=@Description, @Website=@Website, @Category=@Category, " +
                "@Phone=@Phone, @Email=@Email, @IsVerified=@IsVerified, @Status=@Status, " +
                "@AvgRating=@AvgRating, @ReviewCount=@ReviewCount",
                parameters
            );

            // Retrieve newly created business Id
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
                new SqlParameter("@Category", (object?)dto.Category ?? DBNull.Value),
                new SqlParameter("@Phone", (object?)dto.Phone ?? DBNull.Value),
                new SqlParameter("@Email", (object?)dto.Email ?? DBNull.Value),
                new SqlParameter("@IsVerified", dto.IsVerified),
                new SqlParameter("@Status", dto.Status),
                new SqlParameter("@AvgRating", dto.AvgRating),
                new SqlParameter("@ReviewCount", dto.ReviewCount)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_Business @Action=@Action, @Id=@Id, @Name=@Name, @Description=@Description, " +
                "@Website=@Website, @Category=@Category, @Phone=@Phone, @Email=@Email, " +
                "@IsVerified=@IsVerified, @Status=@Status, @AvgRating=@AvgRating, @ReviewCount=@ReviewCount",
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

        public async Task CreateOrUpdateBusinessHourAsync(BusinessHourDto dto, string action)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", action),
                new SqlParameter("@Id", dto.Id),
                new SqlParameter("@BusinessId", dto.BusinessId),
                new SqlParameter("@DayOfWeek", dto.DayOfWeek),
                new SqlParameter("@OpenTime", (object?)dto.OpenTime ?? DBNull.Value),
                new SqlParameter("@CloseTime", (object?)dto.CloseTime ?? DBNull.Value),
                new SqlParameter("@IsClosed", dto.IsClosed)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_BusinessHour @Action=@Action, @Id=@Id, @BusinessId=@BusinessId, " +
                "@DayOfWeek=@DayOfWeek, @OpenTime=@OpenTime, @CloseTime=@CloseTime, @IsClosed=@IsClosed",
                parameters
            );
        }
    }
}
