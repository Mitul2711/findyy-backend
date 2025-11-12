using findyy.DTO.Auth;
using findyy.Model.Auth;

namespace LocalBizFinder.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        public Task<User> GetUserAsync(string email);
        public Task<User> GetUserByIdAsync(Guid id);
        Task RegisterAsync(RegisterDto user);
        public Task<User?> GetUserByTokenAsync(string token);
        public Task VerifyUserAsync(string token);

    }
}
