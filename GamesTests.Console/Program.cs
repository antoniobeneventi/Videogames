using GamesDataAccess;
using GamesDataAccess.Criterias;
using GamesDataAccess.DbItems;
using System.Data.SQLite;

string dbFile = @"..\..\Data\test.db;Foreign Keys=True";
string dbFilePath = Path.GetDirectoryName(dbFile)!;
if (!Directory.Exists(dbFilePath))
{
    Directory.CreateDirectory(dbFilePath);
}

string connStr = $@"Data Source={dbFile}; Version=3;";

GamesDao gamesDao =
    new GamesDao
    (
        () => new SQLiteConnection(connStr),
        "||"
    );

gamesDao.DropAllTables();

gamesDao.CreateAllTables();

DataPopulator dataPopulator = new DataPopulator(gamesDao);

dataPopulator.AddSomeData();

Console.WriteLine(new string('-', 80));

GameDbItem[] games = gamesDao.GetAllGames();

foreach (var game in games)
{
    Console.WriteLine(game);
}

Console.WriteLine(new string('-', 80));

StoreDbItem[] stores = gamesDao.GetAllStores();

foreach (var store in stores)
{
    Console.WriteLine(store);
}

Console.WriteLine(new string('-', 80));

PlatformDbItem[] platforms = gamesDao.GetAllPlatforms();

foreach (var platform in platforms)
{
    Console.WriteLine(platform);
}

// DateOnly.FromDateTime(dataReader.GetDateTime(1));

Console.WriteLine(new string('-', 80));

var ownedGames = 
    gamesDao
    .GetOwnedGamesByCriteria
    (
        new GamesCriteria 
        { 
            PurchaseDateFrom = new DateOnly(2022, 1, 1)
        }
    );

foreach (var tx in ownedGames)
{
    Console.WriteLine(tx);
}

// DateOnly.FromDateTime(dataReader.GetDateTime(1));

Console.WriteLine(new string('-', 80));