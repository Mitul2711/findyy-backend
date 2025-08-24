using findyy.Model.Auth;

namespace LocalBizFinder.DataAccess.Interfaces
{
    public interface IUserRepository
    {
        Task RegisterAsync(User user);
        Task<User?> LoginAsync(string email, string passwordHash);
    }
}
