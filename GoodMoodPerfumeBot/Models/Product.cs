using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.Models
{
    public class Product
    {
        [Key]        
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Category { get; set; }
        [Required]
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        [Range(1, 100000)]
        [Required]
        public double Price { get; set; }
    }
}
