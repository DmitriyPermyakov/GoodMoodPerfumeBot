using GoodMoodPerfumeBot.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodMoodPerfumeBot.DTOs
{
    public class CreateOrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
