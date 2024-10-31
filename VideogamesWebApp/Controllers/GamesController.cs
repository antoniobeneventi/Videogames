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
        var username = GetUsername();


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
        ViewData["Username"] = username;


        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(transactions);
        }

        ViewData["searchQuery"] = searchQuery;

        ViewData["AvailableGames"] = _dbContext.Games
                                      .Select(g => new { g.GameId, g.GameName })
                                      .ToList();

        ViewData["AvailableStores"] = _dbContext.Stores
                                         .Select(s => new { s.StoreId, s.StoreName })
                                         .ToList();
        ViewData["AvailablePlatforms"] = _dbContext.Platforms
                                                   .Select(p => new { p.PlatformId, p.PlatformName })
                                                   .ToList();
        ViewData["AvailableLaunchers"] = _dbContext.Launchers
                                                   .Select(l => new { l.LauncherId, l.LauncherName })
                                                   .ToList();

        return View("~/Views/Home/Index.cshtml", transactions);
    }
    [HttpPost]
    public IActionResult BuyGame(GamePurchaseViewModel model)
    {
        if (ModelState.IsValid)
        {
            var newTransaction = new GameTransactions // Assuming you still want to use this entity to save the transaction
            {
                GameId = model.GameId,
                StoreId = model.StoreId,
                PlatformId = model.PlatformId,
                LauncherId = model.LauncherId,
                PurchaseDate = model.PurchaseDate,
                Price = model.Price,
                IsVirtual = model.IsVirtual,
                UserId = GetUserId(), // Make sure to assign the userId
                Notes = string.IsNullOrEmpty(model.Notes) ? null : model.Notes // Set Notes to null if empty

            };

            _dbContext.GameTransactions.Add(newTransaction);
            _dbContext.SaveChanges();

            return RedirectToAction("Index"); 
        }

        // Repopulate dropdowns again if model state is invalid
        ViewData["AvailableGames"] = _dbContext.Games.Select(g => new { g.GameId, g.GameName }).ToList();
        ViewData["AvailableStores"] = _dbContext.Stores.Select(s => new { s.StoreId, s.StoreName }).ToList();
        ViewData["AvailablePlatforms"] = _dbContext.Platforms.Select(p => new { p.PlatformId, p.PlatformName }).ToList();
        ViewData["AvailableLaunchers"] = _dbContext.Launchers.Select(l => new { l.LauncherId, l.LauncherName }).ToList();

        return View("Index", model); // Reload the form with errors if needed
    }



    public IActionResult ViewAllGames()
    {
        var username = GetUsername();

        var allGamesQuery = from game in _dbContext.Games
                            select new GameViewModel
                            {
                                GameId = game.GameId,
                                GameName = game.GameName,
                                GameDescription = game.GameDescription,
                                MainGameId = game.MainGameId
                            };

        var allGames = allGamesQuery.ToList();
        ViewData["Username"] = username;
        return View("~/Views/Home/ViewAllGames.cshtml", allGames);
    }

    private string GetUsername() 
    {
        return HttpContext.Session.GetString("Username") ?? "Guest";
    }

    private int GetUserId()
    {
        return HttpContext.Session.GetInt32("UserId") ?? 0;
    }
}