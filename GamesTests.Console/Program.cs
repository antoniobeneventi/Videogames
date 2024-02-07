using GamesDataAccess;
using GamesDataAccess.Criterias;
using GamesDataAccess.DbItems;
using System.Data.SQLite;

string dbFile = @"..\..\Data\test.db";
string dbFilePath = Path.GetDirectoryName(dbFile)!;
if (!Directory.Exists(dbFilePath))
{
    Directory.CreateDirectory(dbFilePath);
}

string connStr = $@"Data Source={dbFile}; Version=3;Foreign Keys=True";

GamesDao gamesDao =
    new GamesDao
    (
        connectionFactory: () => new SQLiteConnection(connStr),
        strConcatOperator: "||",
        parameterPrefix: ":"
    );

gamesDao.DropAllTables();

gamesDao.CreateAllTables();

DataPopulator dataPopulator = new DataPopulator(gamesDao);

dataPopulator.AddSomeData();

PrintLine("All games");

GameDbItem[] games = gamesDao.GetAllGames();

foreach (var game in games)
{
    Console.WriteLine(game);
}

PrintLine("All stores");

StoreDbItem[] stores = gamesDao.GetAllStores();

foreach (var store in stores)
{
    Console.WriteLine(store);
}

PrintLine("All platforms");

PlatformDbItem[] platforms = gamesDao.GetAllPlatforms();

foreach (var platform in platforms)
{
    Console.WriteLine(platform);
}

PrintLine("Owned games");

var ownedGames = 
    gamesDao
    .GetOwnedGamesByCriteria
    (
        new GamesCriteria 
        { 
            PurchaseDateFrom = new DateOnly(2022, 1, 1),
            StoreName = "me",
            StoreDescription = "resto",
            PlatformName = "Play",
            GameName = "la",
            GameTags = "adv"
        }
    );

foreach (var tx in ownedGames)
{
    Console.WriteLine(tx);
}

PrintLine("End");

void PrintLine(string? title = null)
{
    const int maxLen = 100;

    if ((title?.Length ?? 0) > maxLen)
    {
        Console.WriteLine(title);
        return;
    }
    
    int halfLen = (maxLen - (title?.Length ?? 0)) / 2;
    string halfLine = new string('-', halfLen);
    Console.WriteLine($"{halfLine} {title ?? ""} {halfLine}");
}