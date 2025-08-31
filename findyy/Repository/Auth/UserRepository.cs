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
            var parameters = new[]
            {
                new SqlParameter("@Action", "POST"),
                new SqlParameter("@Id", Guid.NewGuid()),
                new SqlParameter("@FullName", (object?)user.FullName ?? DBNull.Value),
                new SqlParameter("@Email", (object?)user.Email ?? DBNull.Value),
                new SqlParameter("@PasswordHash", (object?)user.Password ?? DBNull.Value),
                new SqlParameter("@Role", (object?)user.Role ?? DBNull.Value),
                new SqlParameter("@IsEmailVerified", false),
                new SqlParameter("@Status", "Pending")
            };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_RegisterUser @Action=@Action, @Id=@Id, @FullName=@FullName, @Email=@Email, @PasswordHash=@PasswordHash, @Role=@Role, @IsEmailVerified=@IsEmailVerified, @Status=@Status",
                parameters
            );
        }

    }
}
