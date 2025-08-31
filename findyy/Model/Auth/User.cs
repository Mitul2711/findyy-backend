using System.ComponentModel.DataAnnotations;

namespace findyy.Model.Auth
{
    public class User
    {
        [Key] public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = default!;
        public bool IsEmailVerified { get; set; }
        public string Status { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }

}
