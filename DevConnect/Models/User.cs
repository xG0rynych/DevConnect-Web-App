using DevConnect.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevConnect.Models
{
    [Index(nameof(Username), nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public string? Skills { get; set; }
        [Required]
        public DateOnly CreatedAt { get; set; }
        public DateOnly LastOnline { get; set; }
    }
}
