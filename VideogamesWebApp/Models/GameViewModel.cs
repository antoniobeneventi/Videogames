namespace VideogamesWebApp.Models;

public class GameViewModel
{

    public int GameId { get; set; }
    public string GameName { get; set; }
    public string GameDescription { get; set; }
    public string GameTags { get; set; }
    public string? MainGameId { get; set; }

}
