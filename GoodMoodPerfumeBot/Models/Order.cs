using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public long UserId { get; set; }

        public List<OrderItem>? OrderItems { get; set; }
    }
}
