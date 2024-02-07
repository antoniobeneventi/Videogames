namespace GamesDataAccess.DbItems;

public record OwnedGameDbItem
(
    string TransactionId,
    DateOnly PurchaseDate,
    bool IsVirtual,
    string StoreId,
    string StoreName,
    string StoreDescription,
    string PlatformId,
    string PlatformName,
    string PlatformDescription,
    string GameId,
    string GameName,
    string GameDescription,
    string GameTags,
    decimal Price
);
