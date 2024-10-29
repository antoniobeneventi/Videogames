using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models;

public class Launcher
{
    [Required]
    public int LauncherId { get; set; }

    [MaxLength(15)]
    public string LauncherName { get; set; }

    [MaxLength(25)]
    public string LauncherDescription { get; set; }

    [MaxLength(100)]
    public string Link { get; set; }
}
