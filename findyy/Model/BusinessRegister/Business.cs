using findyy.Model.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace findyy.Model.BusinessRegister
{
    public class Business
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid OwnerUserId { get; set; }
        [ForeignKey(nameof(OwnerUserId))]
        public virtual User? Owner { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        public string? Description { get; set; }
        public string? Website { get; set; }

        // 🔹 Replace string Category with reference to BusinessCategory
        [ForeignKey(nameof(BusinessCategory))]
        public long? BusinessCategoryId { get; set; }
        public virtual BusinessCategory? BusinessCategory { get; set; }

        public string? Phone { get; set; }
        public string? Email { get; set; }

        public bool IsVerified { get; set; }
        public byte Status { get; set; }   // 0 = pending, 1 = Active, 2 = rejected, 3 = Blocked
        public decimal AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // --- Navigation Properties ---
        public virtual BusinessLocation? Location { get; set; }
        public virtual ICollection<BusinessHour> Hours { get; set; } = new List<BusinessHour>();
    }

    public class BusinessLocation
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(Business))]
        public long BusinessId { get; set; }

        [Required]
        public string AddressLine1 { get; set; } = default!;
        public string? AddressLine2 { get; set; }
        [Required]
        public string City { get; set; } = default!;
        [Required]
        public string State { get; set; } = default!;
        [Required]
        public string Country { get; set; } = default!;
        public string? PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // --- Navigation Property ---
        [JsonIgnore]
        public virtual Business? Business { get; set; }
    }

    public class BusinessHour
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(Business))]
        public long BusinessId { get; set; }

        public byte DayOfWeek { get; set; } // 0=Sunday..6=Saturday
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsClosed { get; set; }

        [JsonIgnore]
        public virtual Business? Business { get; set; }
    }

    // 🔹 New Table: BusinessCategory
    public class BusinessCategory
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [MaxLength(255)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // --- Navigation Property ---
        [JsonIgnore]
        public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();
    }
}
