using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.Models
{
    public class PaymentDetails
    {
        [Key]
        public int Id { get; set; }
        public string? Phone { get; set; }
        public string? CardNumber { get; set; }
    }
}
