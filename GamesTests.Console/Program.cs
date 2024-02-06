using GamesDataAccess;
using System.Data.SQLite;
using System.Net.Http.Headers;

string dbFile = @"..\..\Data\test.db";
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

Console.WriteLine(new string('-', 80));