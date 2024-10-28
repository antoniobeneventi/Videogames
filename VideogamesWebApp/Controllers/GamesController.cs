namespace VideogamesWebApp.Controllers;

using Microsoft.AspNetCore.Mvc;
using GamesDataAccess;
using VideogamesWebApp.Models;
using System.Linq; 

public class GamesController : Controller
{
    private readonly DatabaseContext _dbContext;

    public GamesController(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Index(string searchQuery)
    {
        var gamesQuery = _dbContext.Games.AsQueryable();

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
