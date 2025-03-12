using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }

    }
}
