using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models
{
    public class Platforms
    {
        [Required]
        public int PlatformId { get; set; }

        [MaxLength(15)]
        public string PlatformName { get; set; }

        [MaxLength(25)]
        public string PlatformDescription { get; set; }
    }
}