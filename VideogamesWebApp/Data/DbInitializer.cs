using GamesDataAccess;
using Microsoft.EntityFrameworkCore;
using VideogamesWebApp.Models;

public static class DbInitializer
{
    public static void Initialize(DatabaseContext context)
    {
        context.Database.EnsureCreated();

        if (!context.Games.Any())
        {
            var games = new Game[]
            {
                new Game 
                {
                    GameId = "Grand_Theft_Auto_V",
                    GameName = "Grand Theft Auto V",
                    GameDescription = "An open-world game",
                    GameTags = "Gun, Violence"
                },

                new Game
                {
                    GameId = "Formula_1", 
                    GameName = "Formula 1", 
                    GameDescription = "A car game.",
                    GameTags = "Car, fast"
                },
                new Game 
                { 
                    GameId = "Call_Of_Duty",
                   GameName = "Call Of Duty",
                    GameDescription = "Shooter game",
                    GameTags = "gun"
                },
                new Game
                {
                    GameId = "fifa", 
                    GameName = "Fifa",
                   GameDescription = "football game",
                    GameTags = "Football, ball"
                },
            };

            context.Games.AddRange(games);
        }

        if (!context.GameTransactions.Any())
        {
            var transactions = new GameTransactions[]
            {
                new GameTransactions
                {
                    TransactionId = 1,
                    PurchaseDate = DateTime.Now,
                    IsVirtual = true,
                    StoreId = 2,
                    PlatformId = 2,
                    GameId = "Grand_Theft_Auto_V",
                    Price = 80,
                    Notes = "First transaction"
                },
                new GameTransactions
                {
                    TransactionId = 2,
                    PurchaseDate = DateTime.Now.AddDays(-5),
                    IsVirtual = false,
                    StoreId = 3,
                    PlatformId = 1,
                    GameId = "Formula_1",
                    Price = 70,
                    Notes = "Second transaction"
                },
                 new GameTransactions
                {
                    TransactionId = 3,
                    PurchaseDate = DateTime.Now.AddDays(-25),
                    IsVirtual = false,
                    StoreId = 1,
                    PlatformId = 3,
                    GameId = "Call_Of_Duty",
                    Price = 100,
                    Notes = "Second transaction"
                },
                  new GameTransactions
                {
                    TransactionId = 4,
                    PurchaseDate = DateTime.Now.AddDays(-10),
                    IsVirtual = false,
                    StoreId = 4,
                    PlatformId = 4,
                    GameId = "fifa",
                    Price = 90,
                    Notes = "Second transaction"
                }
            };

            context.GameTransactions.AddRange(transactions);
        }

        if (!context.Stores.Any())
        {
            var stores = new Stores[]
            {
                new Stores 
                {
                    StoreId = 1,
                    StoreName = "Steam", 
                    StoreDescription = "Online game store for PC games", 
                    StoreLink = "https://store.steampowered.com"
                },
                new Stores 
                { 
                    StoreId = 2,
                    StoreName = "PlayStation Store",
                    StoreDescription = "Sony's official PlayStation store",
                    StoreLink = "https://store.playstation.com" 
                },
                new Stores 
                { StoreId = 3, 
                    StoreName = "Microsoft Store",
                    StoreDescription = "Microsoft's official Xbox and PC store",
                    StoreLink = "https://www.microsoft.com/store"
                },
                 new Stores { 
                     StoreId = 4, 
                     StoreName = "Instant Gaming", 
                     StoreDescription = "An online store to buy games", 
                     StoreLink = "https://www.instant-gaming.com/it"
                 }
            };

            context.Stores.AddRange(stores);
        }

        if (!context.Platforms.Any())
        {
            var platforms = new Platforms[]
            {
                new Platforms 
                { 
                    PlatformId = 1, 
                    PlatformName = "PC", 
                    PlatformDescription = "Personal Computer" 
                },
                new Platforms 
                { 
                    PlatformId = 2, 
                    PlatformName = "PlayStation",
                    PlatformDescription = "Sony PlayStation console"
                },
                new Platforms 
                { 
                    PlatformId = 3,  
                    PlatformName = "Xbox",
                    PlatformDescription = "Microsoft Xbox Console" 
                },
                new Platforms 
                { PlatformId = 4,
                    PlatformName = "Nintendo Switch",
                    PlatformDescription = "Nintendo console" 
                }
            };

            context.Platforms.AddRange(platforms);
        }

        context.SaveChanges();
    }
}










