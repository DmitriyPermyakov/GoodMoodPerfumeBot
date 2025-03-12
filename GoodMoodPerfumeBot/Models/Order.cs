using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public long UserId { get; set; }
        public AppUser? AppUser { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public string? Address { get; set; }
        public string? OrderStatus { get; set; }
        public string? PayStatus { get; set; }

    }
}
