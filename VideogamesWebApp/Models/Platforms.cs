using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models
{
    public class Platforms
    {
        [Key]
        [Required]
        public int PlatformId { get; set; } 
        public string PlatformName { get; set; }
        public string PlatformDescription { get; set; }
    }
}