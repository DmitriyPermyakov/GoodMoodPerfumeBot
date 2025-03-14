﻿using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.Models
{
    public class Product
    {
        [Key]        
        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? ProductDescription { get; set; }
        public List<string>? ProductImageUrls { get; set; } = new List<string>();
        [Range(1, 100000)]
        [Required]
        public double ProductPrice { get; set; }
    }
}
