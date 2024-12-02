using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models;

public class User
{
    [Required]
    public int UserId { get; set; }

    [Required]

    [MaxLength(30)]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }
    public string? ProfileImage { get; set; }
}
