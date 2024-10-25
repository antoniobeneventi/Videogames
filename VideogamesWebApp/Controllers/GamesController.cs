namespace VideogamesWebApp.Controllers;

using Microsoft.AspNetCore.Mvc;
using GamesDataAccess;
using GamesDataAccess.Criterias; // You might not need this anymore
using VideogamesWebApp.Models;
using System.Linq; // Make sure to include LINQ for querying

public class GamesController : Controller
{
    private readonly DatabaseContext _dbContext;

    public GamesController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index(string searchQuery)
    {
        // Query the Games DbSet directly
        var gamesQuery = _dbContext.Games.AsQueryable();

        // Apply filtering if a search query is provided
        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            gamesQuery = gamesQuery.Where(game => game.GameName.Contains(searchQuery));
        }

        var games = gamesQuery
            .Select(game => new GameViewModel
            {
                GameId = game.GameId,
                GameName = game.GameName,
                GameDescription = game.GameDescription,
                GameTags = game.GameTags
            })
            .ToList();

        ViewData["searchQuery"] = searchQuery;

        return View("~/Views/Home/Index.cshtml", games);
    }
}
