using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models;

public class Stores
{
 
    [Required]
    public int StoreId { get; set; }

    [MaxLength(15)]

    public string StoreName { get; set; }

    [MaxLength(15)]

    public string StoreDescription { get; set; }

    [MaxLength(50)]

    public string StoreLink { get; set; }
}
