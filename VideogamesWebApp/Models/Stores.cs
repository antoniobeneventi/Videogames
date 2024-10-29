using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models;

public class Stores
{
    [Key]
    [Required]
    public int StoreId { get; set; }  // String primary key
    public string StoreName { get; set; }
    public string StoreDescription { get; set; }
    public string StoreLink { get; set; }
}
