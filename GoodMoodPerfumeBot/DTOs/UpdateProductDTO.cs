using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.DTOs
{
    public class UpdateProductDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? ProductDescription { get; set; }
        public List<string>? OldImagesUrls { get; set; }
        public IFormFile[]? ProductImageFiles { get; set; }
        [Range(1, 100000)]
        [Required]
        public double ProductPrice { get; set; }
    }
}
