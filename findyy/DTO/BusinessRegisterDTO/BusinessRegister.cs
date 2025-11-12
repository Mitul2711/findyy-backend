using findyy.Model.BusinessRegister;
using System.ComponentModel.DataAnnotations.Schema;

namespace findyy.DTO.Business
{
    public class BusinessDto
    {
        public long Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Website { get; set; }
        public long? BusinessCategoryId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }

    public class BusinessUpdateDto
    {
        public long Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Website { get; set; }
        public long? BusinessCategoryId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsVerified { get; set; }
        public byte Status { get; set; }
        public decimal AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }

    // ---------------- Business Location DTO ----------------
    public class BusinessLocationDto
    {
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

    // ---------------- Business Hour DTO ----------------
    public class BusinessHourDto
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public byte DayOfWeek { get; set; }   // 0 = Sunday ... 6 = Saturday
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }


    // ------------------- Business Verify -------------------
    public class ReviewBusinessDto
    {
        public bool IsVerified { get; set; }
        public byte Status { get; set; } // 0 = Hidden, 1 = Active, 2 = Pending, 3 = Blocked
    }


    public class BusinessResponseDto
    {
        public long Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Website { get; set; }
        public long? BusinessCategoryId { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsVerified { get; set; }
        public byte Status { get; set; }
        public decimal AvgRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public BusinessLocation? Location { get; set; }
        public List<BusinessHour>? Hours { get; set; }
    }

    public class BusinessReviewDTO
    {
        public long Id { get; set; }
        public long BusinessId { get; set; }
        public int RatingStarCount { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewDescription { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }

}
