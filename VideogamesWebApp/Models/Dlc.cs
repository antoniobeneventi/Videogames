using System.ComponentModel.DataAnnotations;
using VideogamesWebApp.Models;

public class Dlc
{
    [Required]
    public int DlcId { get; set; } 
    public string DlcName { get; set; } 
    public string DlcDescription { get; set; } 
    public decimal Price { get; set; } 
    public string GameId { get; set; } 
}
