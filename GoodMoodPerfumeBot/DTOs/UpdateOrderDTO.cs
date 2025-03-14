using GoodMoodPerfumeBot.Models;
using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.DTOs
{
    public class UpdateOrderDTO
    {
        [Required]
        public int OrderId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string? Address { get; set; }
    }
}
