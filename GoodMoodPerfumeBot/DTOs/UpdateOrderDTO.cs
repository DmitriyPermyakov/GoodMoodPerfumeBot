using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.DTOs
{
    public class UpdateOrderDTO
    {
        public int OrderId { get; set; }
        public string? OrderStatus { get; set; }
        public string? PayStatus { get; set; }
        public string? Address { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
