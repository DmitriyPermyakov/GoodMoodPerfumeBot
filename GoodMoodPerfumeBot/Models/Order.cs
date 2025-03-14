using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodMoodPerfumeBot.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey(nameof(AppUser))]
        public long UserId { get; set; }
        public AppUser? AppUser { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string? Address { get; set; }
        public string? OrderStatus { get; set; }
        public string? PayStatus { get; set; }

    }
}
