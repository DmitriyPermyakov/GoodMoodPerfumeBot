using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodMoodPerfumeBot.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [ForeignKey(nameof(AppUser))]
        public int UserId { get; set; }
        public AppUser? AppUser { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Delivery { get; set; }
        public string? OrderStatus { get; set; }
        public string? PayStatus { get; set; }

    }
}
