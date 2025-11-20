namespace findyy.DTO.BusinessPhoto
{
    public class BusinessphotoDTO
    {
        public long BusinessId { get; set; }

        public IFormFile? MainPhoto { get; set; }

        public List<IFormFile>? AdditionalPhotos { get; set; }

        public string? MainCaption { get; set; }

        public List<long>? RemovedPhotoIds { get; set; }
    }
}
