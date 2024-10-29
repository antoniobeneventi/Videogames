using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models;

public class Launcher
{
    [Required]
    public int LauncherId { get; set; }
    public string LauncherName { get; set; }
    public string LauncherDescription { get; set; }
    public string Link { get; set; }
}
