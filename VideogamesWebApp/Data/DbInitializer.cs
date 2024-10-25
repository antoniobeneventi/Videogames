using GamesDataAccess;
using Microsoft.EntityFrameworkCore;
using VideogamesWebApp.Models;

public static class DbInitializer
{
    public static void Initialize(DatabaseContext context)
    {
        context.Database.EnsureCreated();

        if (context.Games.Any())
        {
            return; 
        }

        var games = new Game[]
        {
            new Game { GameId = "The_Witcher_3", GameName = "The Witcher 3", GameDescription = "An open-world RPG set in a fantasy universe.", GameTags = "RPG, Open World" },
            new Game { GameId = "Hollow_Knight", GameName = "Hollow Knight", GameDescription = "A challenging metroidvania with a beautiful hand-drawn style.", GameTags = "Platformer, Indie" },
            new Game { GameId = "Stardew_Valley", GameName = "Stardew Valley", GameDescription = "A farming simulation game that allows you to build your farm.", GameTags = "Simulation, Indie" },
             new Game { GameId = "fifa", GameName = "Fifa", GameDescription = "fifa.", GameTags = "fifa" },

        };

        context.Games.AddRange(games);
        context.SaveChanges();
    }
}
