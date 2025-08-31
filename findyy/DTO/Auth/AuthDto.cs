using System.ComponentModel.DataAnnotations;
using findyy.Model.Auth;

namespace findyy.DTO.Auth
{
    public class RegisterDto
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
    }

    public class LoginDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

}
