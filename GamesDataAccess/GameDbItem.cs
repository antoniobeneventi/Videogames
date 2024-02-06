namespace GamesDataAccess;
// See https://aka.ms/new-console-template for more information
public record GameDbItem
(
    string GameId,
    string GameName,
    string GameDescription,
    string GameTags
);

public record StoreDbItem
(
    string StoreId,
    string StoreName,
    string StoreDescription,
    string StoreLink
);

public record PlatformDbItem
(
    string PlatformId,
    string PlatformName,
    string PlatformDescription
);

public record GameTransactionDbItem
(
    string TransactionId,
    DateOnly PurchaseDate,
    bool IsVirtual,
    string StoreId,
    string PlatformId,
    string GameId,
    decimal Price,
    string Notes
);

