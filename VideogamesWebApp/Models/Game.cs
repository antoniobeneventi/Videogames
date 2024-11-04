using System.ComponentModel.DataAnnotations;

namespace VideogamesWebApp.Models;

public class Game
{
    [Required]
    public int GameId { get; set; }

    [MaxLength(25)]
    public string GameName { get; set; }

    [MaxLength(50)]
    public string GameDescription { get; set; }



    [MaxLength(30)]
    public string? MainGameId { get; set; }

}

