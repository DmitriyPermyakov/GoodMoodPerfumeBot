using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.DTOs
{
    public class UpdateProductDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        [Range(1, 100000)]
        [Required]
        public double Price { get; set; }
    }
}
