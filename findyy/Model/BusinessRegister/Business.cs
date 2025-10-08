using System.ComponentModel.DataAnnotations;

namespace findyy.Model.BusinessRegister
{
    public class Business
    {
        [Key]
        public long Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Website { get; set; }
        public string? Category { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsVerified { get; set; }
        public byte Status { get; set; }   // 0 = Hidden/Deleted, 1 = Active, 2 = Pending, 3 = Blocked
        public decimal AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public BusinessLocation? Location { get; set; }
        public ICollection<BusinessHour> Hours { get; set; } = new List<BusinessHour>();
    }

    public class BusinessLocation
    {
        [Key]
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public string AddressLine1 { get; set; } = default!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string? PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class BusinessHour
    {
        [Key]
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public byte DayOfWeek { get; set; } // 0=Sunday..6=Saturday
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }

}