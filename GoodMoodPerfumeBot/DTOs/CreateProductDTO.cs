using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.DTOs
{
    public class CreateProductDTO
    {        
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? ProductDescription { get; set; }
        [Required]
        public IFormFile[]? Images { get; set; }
        
        [Range(1, 100000)]
        [Required]
        public double ProductPrice { get; set; }
    }
}
