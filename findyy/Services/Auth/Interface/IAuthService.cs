using findyy.DTO.Auth;
using findyy.Model.Response;

namespace findyy.Services.Auth.Interface
{
    public interface IAuthService
    {
        Task<Response> RegisterAsync(UserDto dto);
        Task<Response?> LoginAsync(LoginDto dto);
        public Task<Response> VerifyEmailAsync(string token);
    }
}
