using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace findyy.DTO.BusinessCategoryDTO
{
    public class BusinessCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class BusinessCategoryCountDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // 🔹 Count of businesses in this category
        public int BusinessCount { get; set; }
    }

    public class BusinessCategoryCountDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // 🔹 Count of businesses in this category
        public int BusinessCount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
