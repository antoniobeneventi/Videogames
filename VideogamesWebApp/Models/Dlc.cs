using VideogamesWebApp.Models;

public class DLC
{
    public int DlcId { get; set; } // Identificatore unico per il DLC
    public string DlcName { get; set; } // Nome del DLC
    public string DlcDescription { get; set; } // Descrizione del DLC
    public decimal Price { get; set; } // Prezzo del DLC
    public string GameId { get; set; } // Riferimento al gioco associato
    public Game Game { get; set; } // Navigational property
}
