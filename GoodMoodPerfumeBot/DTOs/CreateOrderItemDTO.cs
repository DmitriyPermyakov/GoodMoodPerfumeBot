using GoodMoodPerfumeBot.Models;

namespace GoodMoodPerfumeBot.DTOs
{
    public class CreateOrderItemDTO
    {
        
        public Order? Order { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
