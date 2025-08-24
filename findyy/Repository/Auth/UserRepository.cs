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

        public async Task RegisterAsync(User user)
        {
            var parameters = new[]
            {
        new SqlParameter("@Id", Guid.NewGuid()),
        new SqlParameter("@FullName", (object?)user.FullName ?? DBNull.Value),
        new SqlParameter("@Email", (object?)user.Email ?? DBNull.Value),
        new SqlParameter("@PasswordHash", (object?)user.PasswordHash ?? DBNull.Value),
        new SqlParameter("@Role", (object?)user.Role.ToString() ?? DBNull.Value),
        new SqlParameter("@IsEmailVerified", false),
        new SqlParameter("@Status", 0)
    };

            await _db.Database.ExecuteSqlRawAsync(
                "EXEC sp_RegisterUser @Id=@Id, @FullName=@FullName, @Email=@Email, @PasswordHash=@PasswordHash, @Role=@Role, @IsEmailVerified=@IsEmailVerified, @Status=@Status",
                parameters
            );
        }


        public async Task<User?> LoginAsync(string email, string passwordHash)
        {
            return await _db.Users
                .FromSqlRaw("EXEC sp_LoginUser @p0, @p1", email, passwordHash)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
