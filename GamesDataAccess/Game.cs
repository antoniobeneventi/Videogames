namespace GamesDataAccess;
// See https://aka.ms/new-console-template for more information
public record Game
(
    string GameId,
    string GameName,
    string GameDescription,
    string GameTags
);