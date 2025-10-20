using findyy.DTO.Auth;
using findyy.Model.Auth;
using LocalBizFinder.DataAccess.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LocalBizFinder.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db) => _db = db;

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _db.Users
                    .FromSqlInterpolated($"EXEC sp_RegisterUser @Action = {"GET"}, @Id = {Guid.NewGuid()}, @Email = {email}")
                    .ToListAsync();

            return user.FirstOrDefault();
        }
        public async Task RegisterAsync(RegisterDto user)
        {
            if (user.BusinessCategoryId == 0)
                user.BusinessCategoryId = null;

            var parameters = new[]
            {
                new SqlParameter("@Action", "POST"),
                new SqlParameter("@Id", Guid.NewGuid()),
                new SqlParameter("@FirstName", (object?)user.FirstName ?? DBNull.Value),
                new SqlParameter("@LastName", (object?)user.LastName ?? DBNull.Value),
                new SqlParameter("@BusinessName", (object?)user.BusinessName ?? DBNull.Value),
                new SqlParameter("@BusinessCategoryId", (object?)user.BusinessCategoryId ?? DBNull.Value),
                new SqlParameter("@City", (object?)user.City ?? DBNull.Value),
                new SqlParameter("@Email", (object?)user.Email ?? DBNull.Value),
                new SqlParameter("@PasswordHash", (object?)user.Password ?? DBNull.Value),
                new SqlParameter("@Role", (object?)user.Role ?? DBNull.Value),
                new SqlParameter("@IsEmailVerified", false),
                new SqlParameter("@Status", "Pending"),
                new SqlParameter("@VerificationToken", (object?)user.EmailVerificationToken ?? DBNull.Value),
                new SqlParameter("@VerificationTokenExpiry", (object?)user.VerificationTokenExpiry ?? DBNull.Value)
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_RegisterUser @Action=@Action, @Id=@Id, @FirstName=@FirstName, @City=@City, @Email=@Email, " +
                "@PasswordHash=@PasswordHash, @Role=@Role, @IsEmailVerified=@IsEmailVerified, " +
                "@LastName=@LastName, @BusinessName=@BusinessName, @BusinessCategoryId=@BusinessCategoryId, " +
                "@Status=@Status, @VerificationToken=@VerificationToken, @VerificationTokenExpiry=@VerificationTokenExpiry",
                parameters
            );
        }

        public async Task<User?> GetUserByTokenAsync(string token)
        {
            var users = await _db.Users
                .FromSqlInterpolated($"EXEC sp_RegisterUser @Action = {"GET_BY_TOKEN"}, @VerificationToken = {token}")
                .ToListAsync();

            return users.FirstOrDefault();
        }

        public async Task VerifyUserAsync(string token)
        {
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC sp_RegisterUser @Action = {"VERIFY"}, @VerificationToken = {token}");
        }


    }
}
