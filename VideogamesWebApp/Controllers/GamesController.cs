using GamesDataAccess;
using Microsoft.AspNetCore.Mvc;
using VideogamesWebApp.Models;

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

        var transactionsQuery = from transaction in _dbContext.GameTransactions
                                join game in _dbContext.Games on transaction.GameId equals game.GameId
                                join store in _dbContext.Stores on transaction.StoreId equals store.StoreId
                                join platform in _dbContext.Platforms on transaction.PlatformId equals platform.PlatformId
                                join launcher in _dbContext.Launchers on transaction.LauncherId equals launcher.LauncherId
                                where transaction.UserId == userId
                                select new GameTransactionsViewModel
                                {
                                    GameId = transaction.GameId,
                                    GameName = game.GameName,
                                    PurchaseDate = transaction.PurchaseDate,
                                    Price = transaction.Price,
                                    IsVirtual = transaction.IsVirtual,
                                    StoreName = store.StoreName,
                                    PlatformName = platform.PlatformName,
                                    LauncherName = launcher.LauncherName,
                                    MainGameId = game.MainGameId
                                };

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            transactionsQuery = transactionsQuery.Where(t =>
                t.GameName.ToLower().Contains(searchQuery) ||
                t.StoreName.ToLower().Contains(searchQuery) ||
                t.PlatformName.ToLower().Contains(searchQuery) ||
                t.LauncherName.ToLower().Contains(searchQuery) ||
                (t.MainGameId != null && t.MainGameId.ToLower().Contains(searchQuery))
            );
        }

        var transactions = transactionsQuery.ToList();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(transactions);
        }

        ViewData["searchQuery"] = searchQuery;
        return View("~/Views/Home/Index.cshtml", transactions);
    }


    public IActionResult ViewAllGames()
    {
        var allGamesQuery = from game in _dbContext.Games
                            select new GameViewModel
                            {
                                GameId = game.GameId,
                                GameName = game.GameName,
                                GameDescription = game.GameDescription, 
                                MainGameId = game.MainGameId
                            };

        var allGames = allGamesQuery.ToList();
        return View("~/Views/Home/ViewAllGames.cshtml", allGames); 
    }

    private int GetUserId()
    {
        return HttpContext.Session.GetInt32("UserId") ?? 0;
    }
}