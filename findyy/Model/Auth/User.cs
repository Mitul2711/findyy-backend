using System.ComponentModel.DataAnnotations;

namespace findyy.Model.Auth
{
    public class User
    {
        [Key] public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsEmailVerified { get; set; }
        public StatusDetails Status { get; set; } = StatusDetails.Pending;
        public DateTime CreatedAt { get; set; }
    }

    public enum UserRole
    {
        User = 0,
        BusinessOwner = 1
    }

    public enum StatusDetails
    {
        Pending = 0,
        Verified = 1
    }

}
