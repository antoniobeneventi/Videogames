
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideogamesWebApp.Models
{
    public class GameTransactions
    {
        [Key]
        public int TransactionId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public bool IsVirtual { get; set; }
        public double Price { get; set; }

        [ForeignKey("Store")]
        public int StoreId { get; set; }
        public Stores Store { get; set; }

        [ForeignKey("Platform")]
        public int PlatformId { get; set; }
        public Platforms Platform { get; set; }

        [ForeignKey("Game")]
        public string GameId { get; set; }
        public Game Game { get; set; }




        public string Notes { get; set; }
    }
}

