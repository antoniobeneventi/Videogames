using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }
}
