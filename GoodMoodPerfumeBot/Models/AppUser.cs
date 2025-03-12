using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GoodMoodPerfumeBot.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }

        public long? TelegramUserId { get; set; }
        public long? ChatId { get; set; }        
        public string? UserRole { get; set; }

    }
}
