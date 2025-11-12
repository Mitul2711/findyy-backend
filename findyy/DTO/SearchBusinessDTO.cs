using findyy.DTO.Business;

namespace findyy.DTO
{
    public class SearchBusinessDTO
    {
        public string? Category { get; set; }
        public string? Location { get; set; }
    }

    public class BusinessSearchResultDto
    {
        public int BusinessId { get; set; }
        public Guid OwnerUserId { get; set; }
        public string BusinessName { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public string Status { get; set; }
        public decimal? AvgRating { get; set; }
        public int? ReviewCount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CategoryName { get; set; }

        // Nested objects
        public BusinessLocationDto Location { get; set; }
        public List<BusinessHourDto> Hours { get; set; }

        public static implicit operator BusinessSearchResultDto?(Model.BusinessRegister.Business? v)
        {
            throw new NotImplementedException();
        }
    }

    public class BusinessLocationDto
    {
        public int LocationId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }

    public class BusinessHourDto
    {
        public int BusinessHourId { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsClosed { get; set; }
    }
}
