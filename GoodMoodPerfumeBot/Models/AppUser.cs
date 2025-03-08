using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.Models
{
    public class AppUser
    {
        [Key]
        public long UserId { get; set; }
        public long? ChatId { get; set; }
        public string? UserRole { get; set; }

    }
}
