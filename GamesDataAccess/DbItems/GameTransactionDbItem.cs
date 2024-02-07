namespace GamesDataAccess.DbItems;

public record GameTransactionDbItem
(
    string TransactionId,
    DateOnly PurchaseDate,
    bool IsVirtual,
    string StoreId,
    string PlatformId,
    string GameId,
    decimal Price,
    string? Notes
);
