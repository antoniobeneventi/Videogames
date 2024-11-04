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
                    GameId = 1,
                    GameName = "Grand Theft Auto V",
                    GameDescription = "An open-world game",
                    GameTags = "Gun, Violence",
                    MainGameId = "Gta online"

                },

                new Game
                {
                    GameId = 2,
                    GameName = "Formula 1",
                    GameDescription = "A car game.",
                    GameTags = "Car, fast",
                    MainGameId = null
                },
                new Game
                {
                    GameId = 3,
                    GameName = "Call Of Duty",
                    GameDescription = "Shooter game",
                    GameTags = "gun",
                    MainGameId = "Awakeing"

                },
                new Game
                {
                    GameId = 4,
                    GameName = "Fifa",
                    GameDescription = "football game",
                    GameTags = "Football, ball",
                    MainGameId = null

                },
                new Game
                {
                   GameId = 5,
                   GameName = "Battlefield",
                   GameDescription = "war game",
                   GameTags = "War, gun, violence",
                   MainGameId = null

                },
                new Game
                {
                   GameId = 6,
                   GameName = "Just dance",
                   GameDescription = "Dance music",
                   GameTags = "Dance, music",
                   MainGameId = "More songs"

                },
                new Game
                {
                   GameId = 7,
                   GameName = "League of Legends",
                   GameDescription = "Lol",
                   GameTags = "Champion, monster",
                   MainGameId = "More champions"
                },
                new Game
                {
                   GameId = 8,
                   GameName = "Valorant",
                   GameDescription = "war game",
                   GameTags = "war, fantasy",
                   MainGameId = null

                },
                new Game
                {
                   GameId = 9,
                   GameName = "Minecraft",
                   GameDescription = "cube game",
                   GameTags = "cube, pixel",
                   MainGameId = null

                },
                new Game
                {
                   GameId = 10,
                   GameName = "Rocket League",
                   GameDescription = "car football game",
                   GameTags = "Football, car",
                   MainGameId = "More car"

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
                    PurchaseDate = DateOnly.FromDateTime(DateTime.Today),
                    IsVirtual = true,
                    StoreId = 2,
                    PlatformId = 2,
                    GameId = 1,
                    Price = 80,
                    Notes = "First transaction",
                    LauncherId = 4,
                    UserId = 1

                },
                new GameTransactions
                {
                    TransactionId = 2,
                    PurchaseDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-5)),
                    IsVirtual = false,
                    StoreId = 3,
                    PlatformId = 1,
                    GameId = 2,
                    Price = 70,
                    Notes = "Second transaction",
                    LauncherId = 3,
                    UserId = 2

                },
                 new GameTransactions
                {
                    TransactionId = 3,
                    PurchaseDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-25)),
                    IsVirtual = false,
                    StoreId = 1,
                    PlatformId = 3,
                    GameId = 3,
                    Price = 100,
                    Notes = "Second transaction",
                    LauncherId = 2,
                    UserId = 2

                },
                  new GameTransactions
                {
                    TransactionId = 4,
                    PurchaseDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-10)),
                    IsVirtual = false,
                    StoreId = 4,
                    PlatformId = 4,
                    GameId = 4,
                    Price = 90,
                    Notes = "Second transaction",
                    LauncherId = 1,
                    UserId = 1


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
                 },
                 new Stores {
                    StoreId = 5,
                    StoreName = "MediaWorld",
                    StoreDescription = "electronics store",
                    StoreLink = "https://www.mediaworld.it"
                 },
                 new Stores {
                    StoreId = 6,
                    StoreName = "Nintendo Store",
                    StoreDescription = "Official Nintendo Store",
                    StoreLink = "https://www.Nintendo.it"
                 },
                 new Stores {
                    StoreId = 7,
                    StoreName = "Games Store",
                    StoreDescription = "The classic phisical store of game",
                    StoreLink = "https://www.GameStop.it"
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



        if (!context.Launchers.Any())
        {
            var launchers = new Launcher[]
            {
        new Launcher
        {
            LauncherId = 1,
            LauncherName = "Steam",
            LauncherDescription = "A popular gaming platform for PC",
            Link = "https://store.steampowered.com"
        },
        new Launcher
        {
            LauncherId = 2,
            LauncherName = "Ubisoft",
            LauncherDescription = "Ubisoft Launcher",
            Link = "https://www.Ubisoft.com"
        },
        new Launcher
        {
            LauncherId = 3,
            LauncherName = "Origin",
            LauncherDescription = "EA's game launcher",
            Link = "https://www.origin.com"
        },
        new Launcher
        {
            LauncherId = 4,
            LauncherName = "Rockstar Games",
            LauncherDescription = "Rockstar Games game launcher",
            Link = "https://www.Rockstar.com"
        }
            };
            context.Launchers.AddRange(launchers);
        }



        context.SaveChanges();
    }
}










