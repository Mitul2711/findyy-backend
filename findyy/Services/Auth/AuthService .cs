using findyy.DTO.Auth;
using findyy.Model.Auth;
using findyy.Services.Auth.Interface;
using LocalBizFinder.Business.Helpers;
using LocalBizFinder.DataAccess.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace LocalBizFinder.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository repo, JwtHelper jwtHelper)
        {
            _repo = repo;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Role = dto.Role
            };

            await _repo.RegisterAsync(user);
            return _jwtHelper.GenerateToken(user);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var hash = HashPassword(dto.Password);
            var user = await _repo.LoginAsync(dto.Email, hash);

            if (user == null) return null;
            return _jwtHelper.GenerateToken(user);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
