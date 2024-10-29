
using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models;

public class GameTransactions
{
    [Required]
    public int TransactionId { get; set; }
    public DateOnly PurchaseDate { get; set; }
    public bool IsVirtual { get; set; }
    public decimal Price { get; set; }
    public int StoreId { get; set; }
    public int PlatformId { get; set; }

    [MaxLength(25)]
    public string GameId { get; set; }
    public int LauncherId { get; set; }
    [MaxLength(25)]
    public string Notes { get; set; }
    public int UserId { get; set; }
}

