
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideogamesWebApp.Models;

public class GameTransactions
{
    [Key]
    [Required]
    public int TransactionId { get; set; }

    public DateOnly PurchaseDate { get; set; }

    public bool IsVirtual { get; set; }
    public decimal Price { get; set; }

    [ForeignKey("Store")]
    public int StoreId { get; set; }

    [ForeignKey("Platform")]
    public int PlatformId { get; set; }

    [ForeignKey("Game")]
    public string GameId { get; set; }

    [ForeignKey("Dlc")]
    public int DlcId { get; set; }


    [ForeignKey("Launcher")]
    public int LauncherId { get; set; }
    public string Notes { get; set; }
}

