using findyy.Model.BusinessRegister;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace findyy.Model.BusinessReview
{
    public class BusinessReview
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey(nameof(Business))]
        public long BusinessId { get; set; }
        public int RatingStarCount { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ReviewTitle { get; set; }
        public string ReviewDescription { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
