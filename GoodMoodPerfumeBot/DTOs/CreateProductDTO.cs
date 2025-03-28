using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.DTOs
{
    public class CreateProductDTO
    {        
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public string? Category { get; set; }
        
        [Range(1, 100000)]
        [Required]
        public double Price { get; set; }
    }
}
