using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models
{
    public class Platforms
    {
        [Key]
        public int PlatformId { get; set; } 
        public string PlatformName { get; set; }
        public string PlatformDescription { get; set; }
    }
}