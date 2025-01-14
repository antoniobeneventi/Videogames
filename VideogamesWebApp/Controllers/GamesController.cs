using GamesDataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideogamesWebApp.Models;
using VideogamesWebApp.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json.Linq;


public class GamesController : Controller
{
    private readonly DatabaseContext _dbContext;
    private readonly StatisticsService _statisticsService;

    public GamesController(DatabaseContext dbContext, StatisticsService statisticsService)
    {
        _dbContext = dbContext;
        _statisticsService = statisticsService;

    }

    public IActionResult Index(string searchQuery, int pageNumber = 100)
    {
        var userId = GetUserId(); 
        var username = GetUsername(); 
        int pageSize = 100;

        var user = _dbContext.Users.SingleOrDefault(u => u.UserId == userId);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var transactionsQuery = from transaction in _dbContext.GameTransactions
                                join game in _dbContext.Games on transaction.GameId equals game.GameId
                                join store in _dbContext.Stores on transaction.StoreId equals store.StoreId
                                join platform in _dbContext.Platforms on transaction.PlatformId equals platform.PlatformId
                                join launcher in _dbContext.Launchers on transaction.LauncherId equals launcher.LauncherId
                                join mainGame in _dbContext.Games on game.MainGameId equals mainGame.GameId into mainGames
                                from mainGame in mainGames.DefaultIfEmpty()
                                where transaction.UserId == userId
                                orderby game.GameName
                                select new GameTransactionsViewModel
                                {
                                    TransactionId = transaction.TransactionId,
                                    GameId = transaction.GameId,
                                    GameName = game.GameName,
                                    PurchaseDate = transaction.PurchaseDate,
                                    Price = transaction.Price,
                                    IsVirtual = transaction.IsVirtual,
                                    StoreName = store.StoreName,
                                    PlatformName = platform.PlatformName,
                                    LauncherName = launcher.LauncherName,
                                    MainGameId = game.MainGameId,
                                    MainGameName = mainGame != null ? mainGame.GameName : null,
                                    CoverImageUrl = game.CoverImageUrl
                                };

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            transactionsQuery = transactionsQuery.Where(t =>
                t.GameName.ToLower().Contains(searchQuery) ||
                t.StoreName.ToLower().Contains(searchQuery) ||
                t.PlatformName.ToLower().Contains(searchQuery) ||
                t.LauncherName.ToLower().Contains(searchQuery) ||
                (t.MainGameName != null && t.MainGameName.ToLower().Contains(searchQuery))
            );
        }

        // Paginazione
        var totalTransactions = transactionsQuery.Count();
        var transactions = transactionsQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();


        // Passa i dati alla vista
        ViewData["Username"] = username;
        ViewData["searchQuery"] = searchQuery;
        ViewData["TotalPages"] = (int)Math.Ceiling((double)totalTransactions / pageSize);
        ViewData["CurrentPage"] = pageNumber;
        ViewData["ProfileImage"] = user.ProfileImage;

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

            var newTransaction = new GameTransactions
            {
                GameId = model.GameId,
                StoreId = model.StoreId,
                PlatformId = model.PlatformId,
                LauncherId = model.LauncherId,
                PurchaseDate = model.PurchaseDate,
                Price = model.Price,
                IsVirtual = model.IsVirtual,
                UserId = GetUserId(),
                Notes = string.IsNullOrEmpty(model.Notes) ? null : model.Notes
            };

            _dbContext.GameTransactions.Add(newTransaction);
            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Purchase completed successfully.";
            return RedirectToAction("Index");
        }

        TempData["ErrorMessage"] = "An error occurred during the purchase. Check the data entered.";
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> ViewAllGames(int pageNumber = 1, string sortOrder = "GameNameAsc")
    {
        int pageSize = 100;
        var username = GetUsername();

        // Ottieni i giochi e il numero totale di giochi dall'API
        var (games, totalGames) = await FetchGamesFromRawgApi(pageNumber, pageSize);

        // Ordina i giochi in base alla richiesta
        games = sortOrder switch
        {
            "GameNameAsc" => games.OrderBy(g => g.GameName).ToList(),
            "GameNameDesc" => games.OrderByDescending(g => g.GameName).ToList(),
            _ => games
        };

        // Calcola il numero totale di pagine
        int totalPages = (int)Math.Ceiling((double)totalGames / pageSize);

        // Passa i dati alla vista
        ViewData["Username"] = username;
        ViewData["TotalPages"] = totalPages;
        ViewData["CurrentPage"] = pageNumber;
        ViewData["CurrentSortOrder"] = sortOrder;

        return View("~/Views/Home/ViewAllGames.cshtml", games);
    }

    private async Task<(List<GameViewModel> Games, int TotalGames)> FetchGamesFromRawgApi(int page, int pageSize)
    {
        using var client = new HttpClient();
        var games = new List<GameViewModel>();
        int totalGames = 0;

        try
        {
            var url = $"https://api.rawg.io/api/games?key=d87d6329a7464628ad26fdb9ab180cbe&page={page}&page_size={pageSize}";
            var response = await client.GetStringAsync(url);

            var jsonResponse = JObject.Parse(response);
            totalGames = jsonResponse["count"]?.ToObject<int>() ?? 0; // Numero totale di giochi nell'API

            var results = jsonResponse["results"]?.ToObject<List<JObject>>();

            if (results != null)
            {
                foreach (var result in results)
                {
                    var game = new GameViewModel
                    {
                        GameId = result["id"]?.ToString(),
                        GameName = result["name"]?.ToString() ?? "Unknown",
                        GameDescription = result["description_raw"]?.ToString() ?? "No description available",
                        CoverImageUrl = result["background_image"]?.ToString() ?? "/images/cover/controller.jpg",
                        DLCCount = 0,
                        MainGameName = null,
                        IsImported = true
                    };

                    games.Add(game);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching data from RAWG API: {ex.Message}");
        }

        return (games, totalGames);
    }



    public async Task<IActionResult> ViewStats(int? storeId, int? platformId, int? launcherId)
    {
        var userId = GetUserId(); // Funzione per ottenere l'ID dell'utente loggato
        var stats = await _statisticsService.GetFilteredStatisticsAsync(userId, storeId, platformId, launcherId);

        ViewData["TotalSpent"] = stats.TotalSpent;
        ViewData["TotalGames"] = stats.TotalGames;
        ViewData["AveragePrice"] = stats.AveragePrice.ToString("F2");
        ViewData["MostActiveDay"] = stats.MostActiveDay;
        ViewData["PercentageVirtual"] = stats.PercentageVirtual;
        ViewData["MostExpensiveGame"] = stats.MostExpensiveGame;
        ViewData["LastPurchase"] = stats.LastPurchase;
        ViewData["ActiveDaysOfWeek"] = stats.ActiveDaysOfWeek;
        ViewData["ActiveMonths"] = stats.ActiveMonths;

        return View("~/Views/Home/ViewStats.cshtml");
    }


    private string GetUsername()
    {
        return HttpContext.Session.GetString("Username") ?? "Guest";
    }

    private int GetUserId()
    {
        return HttpContext.Session.GetInt32("UserId") ?? 0;
    }

    [HttpPost]
    public async Task<IActionResult> AddGame(string gameName, string gameDescription, int? mainGameId, bool fromViewAllGames = false)
    {
        if (!string.IsNullOrWhiteSpace(gameName) && !string.IsNullOrWhiteSpace(gameDescription))
        {
            var existingGame = await _dbContext.Games
            .FirstOrDefaultAsync(g => EF.Functions.Like(g.GameName, gameName));

            if (existingGame != null)
            {
                TempData["ErrorMessage"] = "A game with the same name. Enter different game.";
                return RedirectToAction("ViewAllGames", new { sortOrder = "alphabetical" });
            }

            var game = new Game
            {
                GameName = gameName,
                GameDescription = gameDescription,
                MainGameId = mainGameId,
                IsImported = false, 
                CoverImageUrl = "/images/cover/controller.jpg"

            };

            _dbContext.Games.Add(game);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "The game has been successfully added!";

            if (fromViewAllGames)
            {
                return RedirectToAction("index", "Games", new { sortOrder = "alphabetical", gameName = gameName, gameId = game.GameId });
            }
            else
            {
                return RedirectToAction("ViewAllGames", "Games");
            }
        }

        TempData["ErrorMessage"] = "The game name and description cannot be empty.";
        return RedirectToAction("ViewAllGames", new { sortOrder = "alphabetical" });
    }

    [HttpPost]
    public IActionResult AddStore(string storeName, string storeDescription, string storeLink)
    {
        if (!string.IsNullOrWhiteSpace(storeName))
        {
            var existingStore = _dbContext.Stores.FirstOrDefault(s => s.StoreName.ToLower() == storeName.ToLower());
            if (existingStore != null)
            {
                return Json(new { success = false, message = "Store with this name already exists. Please enter a different name." });
            }

            var newStore = new Stores
            {
                StoreName = storeName,
                StoreDescription = storeDescription,
                StoreLink = storeLink
            };

            _dbContext.Stores.Add(newStore);
            _dbContext.SaveChanges();

            return Json(new
            {
                success = true,
                storeId = newStore.StoreId,
                storeName = newStore.StoreName,
                message = "Store added successfully!"
            });
        }

        return Json(new { success = false, message = "Store name cannot be empty." });
    }


    [HttpPost]
    public IActionResult AddPlatform(string platformName, string platformDescription)
    {
        if (!string.IsNullOrWhiteSpace(platformName))
        {
            var existingPlatform = _dbContext.Platforms
                .FirstOrDefault(p => p.PlatformName.ToLower() == platformName.ToLower());

            if (existingPlatform != null)
            {
                return Json(new { success = false, message = "A platform with this name already exists. Please enter a different name." });
            }

            var newPlatform = new Platforms
            {
                PlatformName = platformName,
                PlatformDescription = platformDescription
            };

            _dbContext.Platforms.Add(newPlatform);
            _dbContext.SaveChanges();
            return Json(new
            {
                success = true,
                platformId = newPlatform.PlatformId,
                platformName = newPlatform.PlatformName,
                message = "Platform added successfully!"
            });
        }
        return Json(new { success = false, message = "Platform name cannot be empty." });
    }

    [HttpPost]
    public IActionResult AddLauncher(string launcherName, string launcherDescription, string link)
    {
        if (!string.IsNullOrWhiteSpace(launcherName))
        {
            var existingLauncher = _dbContext.Launchers
                .FirstOrDefault(l => l.LauncherName.ToLower() == launcherName.ToLower());

            if (existingLauncher != null)
            {
                return Json(new { success = false, message = "A launcher with this name already exists. Please enter a different name." });
            }

            var newLauncher = new Launcher
            {
                LauncherName = launcherName,
                LauncherDescription = launcherDescription,
                Link = link
            };

            _dbContext.Launchers.Add(newLauncher);
            _dbContext.SaveChanges();
            return Json(new
            {
                success = true,
                launcherId = newLauncher.LauncherId,
                launcherName = newLauncher.LauncherName,
                message = "Launcher added successfully!"
            });
        }
        return Json(new { success = false, message = "Launcher name cannot be empty." });
    }

    [HttpPost]
    public IActionResult DeleteGame(int transactionId)
    {
        var transaction = _dbContext.GameTransactions.SingleOrDefault(t => t.TransactionId == transactionId);

        if (transaction == null)
        {
            TempData["ErrorMessage"] = "Transaction not found or already deleted.";
            return RedirectToAction("Index");
        }

        _dbContext.GameTransactions.Remove(transaction);
        _dbContext.SaveChanges();

        TempData["SuccessMessage"] = "Game deleted successfully.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteAllGame(int GameId)
    {
        var game = _dbContext.Games.SingleOrDefault(t => t.GameId == GameId);

        if (game == null)
        {
            TempData["ErrorMessage"] = "Game not found or already deleted.";
            return RedirectToAction("Index");
        }

        // Verifica se il gioco ha DLC associati
        if (_dbContext.Games.Any(dlc => dlc.MainGameId == game.GameId))
        {
            TempData["ErrorMessage"] = "Cannot delete this game because it has at least one associated DLC.";
            return RedirectToAction("ViewAllGames");
        }

        _dbContext.Games.Remove(game);
        _dbContext.SaveChanges();

        TempData["SuccessMessage"] = "Game deleted successfully.";
        return RedirectToAction("ViewAllGames");
    }

    [HttpGet]
    public IActionResult SearchGames(string query)
    {
        var queryWords = query.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var games = _dbContext.Games
            .Where(g => queryWords.All(word => g.GameName.ToLower().Contains(word)))
            .AsEnumerable()
            .Where(g => queryWords.All(word =>
                g.GameName.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(gameName => gameName.StartsWith(word))))
            .Select(g => new { gameId = g.GameId, gameName = g.GameName })
            .Take(10)
            .ToList();

        return Json(games);
    }

    [HttpGet]
    public IActionResult SearchStores(string query)
    {
        var queryWords = query.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var stores = _dbContext.Stores
            .Where(s => queryWords.All(word => s.StoreName.ToLower().Contains(word)))
            .AsEnumerable()
            .Where(s => queryWords.All(word =>
                s.StoreName.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(storeName => storeName.StartsWith(word))))
            .Select(s => new { storeId = s.StoreId, storeName = s.StoreName })
            .Take(10)
            .ToList();

        return Json(stores);
    }

    [HttpGet]
    public IActionResult SearchPlatforms(string query)
    {
        var queryWords = query.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var platforms = _dbContext.Platforms
            .Where(p => queryWords.All(word => p.PlatformName.ToLower().Contains(word)))
            .AsEnumerable()
            .Where(p => queryWords.All(word =>
                p.PlatformName.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(platformName => platformName.StartsWith(word))))
            .Select(p => new { platformId = p.PlatformId, platformName = p.PlatformName })
            .Take(10)
            .ToList();

        return Json(platforms);
    }

    [HttpGet]
    public IActionResult SearchLaunchers(string query)
    {
        var queryWords = query.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        var launchers = _dbContext.Launchers
            .Where(l => queryWords.All(word => l.LauncherName.ToLower().Contains(word)))
            .AsEnumerable()
            .Where(l => queryWords.All(word =>
                l.LauncherName.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(launcherName => launcherName.StartsWith(word))))
            .Select(l => new { launcherId = l.LauncherId, launcherName = l.LauncherName })
            .Take(10)
            .ToList();

        return Json(launchers);
    }

    [HttpGet]
    public IActionResult GetDLCs(int mainGameId, int page = 1, int pageSize = 5)
    {
        var dlcsQuery = _dbContext.Games
                        .Where(g => g.MainGameId == mainGameId)
                        .Select(g => new { g.GameId, g.GameName });

        int totalDLCs = dlcsQuery.Count();
        int totalPages = (int)Math.Ceiling((double)totalDLCs / pageSize);

        var dlcs = dlcsQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

        return Json(new
        {
            dlcs = dlcs,
            currentPage = page,
            totalPages = totalPages
        });
    }

    public IActionResult GetAllStores(int page = 1, int pageSize = 5)
    {
        var stores = _dbContext.Stores.ToList();
        var totalStores = stores.Count();
        var pagedStores = stores.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var result = new
        {
            stores = pagedStores,
            totalCount = totalStores,
            currentPage = page,
            totalPages = (int)Math.Ceiling(totalStores / (double)pageSize)
        };

        return Json(result);
    }

    public IActionResult GetAllPlatforms(int page = 1, int pageSize = 5)
    {
        var platforms = _dbContext.Platforms.ToList();
        var totalPlatforms = platforms.Count();
        var pagedPlatforms = platforms.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var result = new
        {
            platforms = pagedPlatforms,
            TotalCount = totalPlatforms,
            currentPage = page,
            totalPages = (int)Math.Ceiling(totalPlatforms / (double)pageSize)
        };

        return Json(result);
    }

    public IActionResult GetAllLaunchers(int page = 1, int pageSize = 5)
    {
        var launchers = _dbContext.Launchers
            .Select(l => new
            {
                l.LauncherId,
                l.LauncherName,
                l.LauncherDescription,
                l.Link
            })
            .ToList();

        var totalLaunchers = launchers.Count();
        var pagedLaunchers = launchers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var result = new
        {
            launchers = pagedLaunchers,
            TotalCount = totalLaunchers,
            currentPage = page,
            totalPages = (int)Math.Ceiling(totalLaunchers / (double)pageSize)
        };

        return Json(result);
    }

    [HttpGet]
    public JsonResult CheckDuplicatePurchase(int gameId, int storeId)
    {
        var userId = GetUserId();
        var isDuplicate = _dbContext.GameTransactions
            .Any(t => t.GameId == gameId && t.StoreId == storeId && t.UserId == userId);

        return Json(new { isDuplicate });
    }
    public IActionResult GeneratePdf()
    {
        var userId = GetUserId();

        var transactions = (from transaction in _dbContext.GameTransactions
                            join game in _dbContext.Games on transaction.GameId equals game.GameId
                            join store in _dbContext.Stores on transaction.StoreId equals store.StoreId
                            join platform in _dbContext.Platforms on transaction.PlatformId equals platform.PlatformId
                            join launcher in _dbContext.Launchers on transaction.LauncherId equals launcher.LauncherId
                            where transaction.UserId == userId
                            select new GameTransactionsViewModel
                            {
                                TransactionId = transaction.TransactionId,
                                GameId = transaction.GameId,
                                GameName = game.GameName,
                                PurchaseDate = transaction.PurchaseDate,
                                Price = transaction.Price,
                                IsVirtual = transaction.IsVirtual,
                                StoreName = store.StoreName,
                                PlatformName = platform.PlatformName,
                                LauncherName = launcher.LauncherName,
                                MainGameId = game.MainGameId,
                                MainGameName = game.MainGame != null ? game.MainGame.GameName : null,
                                CoverImageUrl = game.CoverImageUrl
                            }).ToList();

        using (var memoryStream = new MemoryStream())
        {
            var document = new iTextSharp.text.Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
            document.Add(new Paragraph("My Game Purchases", titleFont));
            document.Add(new Paragraph(" ")); 

            var table = new PdfPTable(5);
            table.WidthPercentage = 100; 
            table.SpacingBefore = 20f; 
            table.SpacingAfter = 20f; 

            table.AddCell("Game Name");
            table.AddCell("Purchase Date");
            table.AddCell("Price (€)");
            table.AddCell("Store");
            table.AddCell("Platform");

            foreach (var transaction in transactions)
            {
                table.AddCell(transaction.GameName);
                table.AddCell(transaction.PurchaseDate.ToString("dd/MM/yyyy"));
                table.AddCell(transaction.Price.ToString("F2"));
                table.AddCell(transaction.StoreName);
                table.AddCell(transaction.PlatformName);
            }

            document.Add(table);

            var footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            document.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), footerFont));
            document.Add(new Paragraph(" ")); 

            document.Close();

            var fileContent = memoryStream.ToArray();
            return File(fileContent, "application/pdf", "MyGames.pdf");
        }
    }

    public IActionResult GenerateStatsPdf()
    {
        var userId = GetUserId();
        var stats = _statisticsService.GetFilteredStatisticsAsync(userId, null, null, null).Result;

        using (var memoryStream = new MemoryStream())
        {
            var document = new iTextSharp.text.Document();
            PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
            document.Add(new Paragraph("Personal Statistics", titleFont));
            document.Add(new Paragraph(" "));

            document.Add(new Paragraph($"Total Spent: {stats.TotalSpent} €"));
            document.Add(new Paragraph($"Total Games: {stats.TotalGames}"));
            document.Add(new Paragraph($"Average Price: {stats.AveragePrice:F2} €"));
            document.Add(new Paragraph($"Most Active Day: {stats.MostActiveDay}"));
            document.Add(new Paragraph($"Percentage of Virtual Games: {stats.PercentageVirtual}"));
            document.Add(new Paragraph($"Most Expensive Game: {stats.MostExpensiveGame}"));
            document.Add(new Paragraph($"Last Purchase: {stats.LastPurchase}"));
            document.Add(new Paragraph(" "));

            document.Add(new Paragraph("Shopping by Days of the Week:"));
            var daysOfWeekTable = new PdfPTable(2);
            daysOfWeekTable.AddCell(new PdfPCell(new Phrase("Day of the Week")));
            daysOfWeekTable.AddCell(new PdfPCell(new Phrase("Count")));
            document.Add(new Paragraph(" "));


            foreach (var day in stats.ActiveDaysOfWeek)
            {
                daysOfWeekTable.AddCell(new PdfPCell(new Phrase(day.Day.ToString())));
                daysOfWeekTable.AddCell(new PdfPCell(new Phrase(day.Count.ToString())));
            }
            document.Add(daysOfWeekTable);


            document.Add(new Paragraph("Purchases by Months:"));
            var monthsTable = new PdfPTable(3);
            monthsTable.AddCell(new PdfPCell(new Phrase("Year")));
            monthsTable.AddCell(new PdfPCell(new Phrase("Month")));
            monthsTable.AddCell(new PdfPCell(new Phrase("Count")));
            document.Add(new Paragraph(" "));


            foreach (var month in stats.ActiveMonths)
            {
                monthsTable.AddCell(new PdfPCell(new Phrase(month.Year.ToString())));
                monthsTable.AddCell(new PdfPCell(new Phrase(DateTimeFormatInfo.CurrentInfo.GetMonthName(month.Month))));
                monthsTable.AddCell(new PdfPCell(new Phrase(month.Count.ToString()))); 
            }
                document.Add(monthsTable);

                var footerFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                document.Add(new Paragraph("Generated on: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), footerFont));
                document.Add(new Paragraph(" ")); 

                document.Close();

                var fileContent = memoryStream.ToArray();
                return File(fileContent, "application/pdf", "PersonalStatistics.pdf");
            }
        }
    [HttpPost]
    public IActionResult ImportGames(IFormFile csvFile)
    {
        if (csvFile == null || csvFile.Length == 0)
        {
            TempData["ErrorMessage"] = "Please select a valid CSV file.";
            return RedirectToAction("Index");
        }

        try
        {
            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true,
            }))
            {
                var games = csv.GetRecords<GameCsvModel>().ToList();

                foreach (var game in games)
                {

                    if (!_dbContext.Games.Any(g => g.GameName.ToLower() == game.GameName.ToLower()))
                    {
                        var newGame = new Game
                        {
                            GameName = game.GameName,
                            GameDescription = game.GameDescription,
                            IsImported = true,
                            CoverImageUrl = string.IsNullOrEmpty(game.CoverImageUrl) ? "/images/default-cover.jpg" : game.CoverImageUrl
                        };

                        _dbContext.Games.Add(newGame);
                    }
                }

                _dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Games imported successfully!";
            }
        }

        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"An error occurred during import: {ex.Message}";
        }

        return RedirectToAction("Index");
    }


}