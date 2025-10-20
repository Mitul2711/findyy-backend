using findyy.Model.Auth;
using findyy.Model.BusinessRegister;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace findyy.DTO.Auth
{
    public class RegisterDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? BusinessName { get; set; } = default!;
        //public string? BusinessCategory { get; set; } = default!;
        [ForeignKey(nameof(BusinessCategory))]
        public long? BusinessCategoryId { get; set; }
        public virtual BusinessCategory? BusinessCategory { get; set; }
        public string? City { get; set; } = default!;
        public string? Phone { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string? EmailVerificationToken { get; set; }
        public DateTime? VerificationTokenExpiry { get; set; }
    }

    public class UserDto
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? BusinessName { get; set; } = default!;
        //public string? BusinessCategory { get; set; } = default!;

        [ForeignKey(nameof(BusinessCategory))]
        public long? BusinessCategoryId { get; set; }
        public virtual BusinessCategory? BusinessCategory { get; set; }
        public string? City { get; set; } = default!;
        public string? Phone { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
    }

    public class LoginDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class notificationDTO
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string BusinessName { get; set; } = default!;
        public string BusinessDescription { get; set; } = default!;
    }
}
