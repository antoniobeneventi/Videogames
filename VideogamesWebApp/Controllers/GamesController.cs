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
        var userId = GetUserId();

        var gamesQuery = from game in _dbContext.Games
                         join transaction in _dbContext.GameTransactions
                         on game.GameId equals transaction.GameId
                         where transaction.UserId == userId
                         select new GameViewModel
                         {
                             GameId = game.GameId,
                             GameName = game.GameName,
                             GameDescription = game.GameDescription,
                             GameTags = game.GameTags
                         };

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            gamesQuery = gamesQuery.Where(game =>
                game.GameName.ToLower().StartsWith(searchQuery) ||
                game.GameDescription.ToLower().StartsWith(searchQuery) ||
                game.GameTags.ToLower().Contains(searchQuery)
            );
        }

        var games = gamesQuery.ToList();

        ViewData["searchQuery"] = searchQuery;

        return View("~/Views/Home/Index.cshtml", games);
    }

    // This is a placeholder for the method to get the current user's ID
    private int GetUserId()
    {
        return HttpContext.Session.GetInt32("UserId") ?? 0; // Example using session
    }
}
