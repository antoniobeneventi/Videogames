﻿namespace VideogamesWebApp.Models;

public class GameViewModel
{

    public string GameId { get; set; } 
    public string GameName { get; set; }
    public string GameDescription { get; set; }
    public int? MainGameId { get; set; }
    public string MainGameName { get; set; } 
    public int DLCCount { get; set; }
    public bool IsImported { get; set; }
    public string CoverImageUrl { get; set; } 


}
