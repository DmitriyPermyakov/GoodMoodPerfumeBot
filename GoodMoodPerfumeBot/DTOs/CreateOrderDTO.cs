using GoodMoodPerfumeBot.Models;
using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.DTOs
{
    public class CreateOrderDTO
    {
        [Required]        
        
        public long TelegramUserId { get; set; }
        [Required]
        public List<CreateOrderItemDTO>? OrderItems { get; set; }
        [Required]
        public string? Address { get; set; }        
    }
}
