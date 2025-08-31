using findyy.DTO.Auth;
using findyy.Model.Response;

namespace findyy.Services.Auth.Interface
{
    public interface IAuthService
    {
        Task<Response> RegisterAsync(RegisterDto dto);
        Task<Response?> LoginAsync(LoginDto dto);
    }
}
