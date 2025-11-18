namespace findyy.DTO.BusinessPhoto
{
    public class BusinessphotoDTO
    {
        public long BusinessId { get; set; }
        public IFormFile MainPhoto { get; set; } = default!;
        public List<IFormFile>? AdditionalPhotos { get; set; }
        public string? MainCaption { get; set; }
    }
}
