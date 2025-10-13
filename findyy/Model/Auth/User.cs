using findyy.Model.BusinessRegister;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace findyy.Model.Auth
{
    public class User
    {
        [Key] public Guid Id { get; set; }
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
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = default!;
        public bool IsEmailVerified { get; set; }
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? VerificationTokenExpiry { get; set; }

    }

}
