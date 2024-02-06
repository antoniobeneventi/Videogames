using GamesDataAccess;
using System.Data.SQLite;

string dbFile = @"..\..\Data\test.db";
string dbFilePath = Path.GetDirectoryName(dbFile)!;
if (!Directory.Exists(dbFilePath))
{
    Directory.CreateDirectory(dbFilePath);
}

string connStr = $@"Data Source={dbFile}; Version=3;";

GamesDao gamesDal =
    new GamesDao
    (
        () => new SQLiteConnection(connStr),
        "||"
    );

gamesDal.DropAllTables();

gamesDal.CreateAllTables();

//int affected = 
//    gamesDal
//    .AddNewGame
//    (
//        new Game
//        (
//            "zelda-botw",
//            "The Legend of Zelda Breath of the Wild",
//            "The best Zelda of all time?",
//            "zelda;nintendo;gdr;adventure"
//        )
//    );

//    affected =
//    gamesDal
//    .AddNewGame
//    (
//        new Game
//        (
//            "elden-ring",
//            "Elden Ring",
//            "GOTY 2022",
//            "soulslike;gdr;adventure"
//        )
//    );

//Console.WriteLine($"Added {affected} game(s)");

//var allGames = gamesDal.GetAllGames();

//foreach (var game in allGames)
//{
//    Console.WriteLine(game);
//}

Game[] games = gamesDal.GetGamesByPartialName("zel", "gdr");

foreach (var game in games)
{
    Console.WriteLine(game);
}