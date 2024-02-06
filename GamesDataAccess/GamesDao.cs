namespace GamesDataAccess;

using System.Data.Common;

public class GamesDao
{
    private Func<DbConnection> _connectionFactory;
    private string _strConcatOperator;

    public GamesDao
    (
        Func<DbConnection> connectionFactory,
        string strConcatOperator
    )
    {
        _connectionFactory = connectionFactory;
        _strConcatOperator = strConcatOperator;
    }

    private void OpenAndExecute(Action<DbConnection> action)
    {
        using DbConnection conn = _connectionFactory();

        conn.Open();

        action(conn);
    }

    private void ExecuteNonQuery(string commandText) =>
        ExecuteNonQuery(commandText, null);


    public void CreateTableGames()
    {
        string createTableText = $@"
create table games
(
    game_id varchar(20) primary key,
    game_name varchar(255),
    game_description varchar(1024),
    game_tags varchar(5000)
)
";
        ExecuteNonQuery(createTableText);
    }

    public void DropTableGames()
    {
        string dropTableText = $@"drop table games";
        ExecuteNonQuery(dropTableText);
    }

    public void CreateTableStore()
    {
        string createTableText = $@"
create table stores
(
    store_id varchar(20) primary key,
    store_name varchar(255),
    store_description varchar(1024),
    store_link varchar(1024)
)
";
        ExecuteNonQuery(createTableText);
    }

    public void DropTableStore()
    {
        string dropTableText = $@"drop table stores";
        ExecuteNonQuery(dropTableText);
    }

    public void CreateTablePlatform()
    {
        string createTableText = $@"
create table platforms
(
    platform_id varchar(20) primary key,
    platform_name varchar(255),
    platform_description varchar(1024)
)
";
        ExecuteNonQuery(createTableText);
    }

    public void DropTablePlatform()
    {
        string dropTableText = $@"drop table platforms";
        ExecuteNonQuery(dropTableText);
    }

    public void CreateTableTransaction()
    {
        string createTableText = $@"
create table game_transactions
(
    transaction_id varchar(20) not null primary key,
    purchase_date datetime not null,
    is_virtual int not null,
    store_id  varchar(20) not null references stores(store_id),
    platform_id varchar(20) not null references platforms(platform_id),
    game_id varchar(20) not null references games(game_id),
    price decimal(10, 2) not null,
    notes clob null,
    check (price >= 0)
)
";
        ExecuteNonQuery(createTableText);
    }

    public void DropTableTransaction()
    {
        string dropTableText = $@"drop table game_transactions";
        ExecuteNonQuery(dropTableText);
    }

    public void CreateAllTables()
    {
        CreateTableGames();
        CreateTablePlatform();
        CreateTableStore();
        CreateTableTransaction();
    }

    public void DropAllTables()
    {
        Action action = DropTableTransaction;
        action.SafeExecute();

        action = DropTableGames;
        action.SafeExecute();

        action = DropTablePlatform;
        action.SafeExecute();

        action = DropTableStore;
        action.SafeExecute();
    }

    public int AddNewGame(GameDbItem game)
    {
        string cmdText = $@"
    insert into games
    (
        game_id,
        game_name,
        game_description,
        game_tags
    ) 
    values
    (
        :game_id,
        :game_name,
        :game_description,
        :game_tags
    )
    ";

        Action<DbCommand> action =
            cmd =>
            {
                cmd.AddParameterWithValue("game_id", game.GameId);
                cmd.AddParameterWithValue("game_name", game.GameName);
                cmd.AddParameterWithValue("game_description", game.GameDescription);
                cmd.AddParameterWithValue("game_tags", game.GameTags);
            };

        return ExecuteNonQuery(cmdText, action);
    }

    public int AddNewStore(StoreDbItem store)
    {
        string cmdText = $@"
    insert into stores
    (
        store_id,
        store_name,
        store_description,
        store_link
    ) 
    values
    (
        :store_id,
        :store_name,
        :store_description,
        :store_link
    )
    ";

        Action<DbCommand> action =
            cmd =>
            {
                cmd.AddParameterWithValue("store_id", store.StoreId);
                cmd.AddParameterWithValue("store_name", store.StoreName);
                cmd.AddParameterWithValue("store_description", store.StoreDescription);
                cmd.AddParameterWithValue("store_link", store.StoreLink);
            };

        return ExecuteNonQuery(cmdText, action);
    }

    public int AddNewPlatform(PlatformDbItem platform)
    {
        string cmdText = $@"
    insert into platforms
    (
        platform_id,
        platform_name,
        platform_description
    ) 
    values
    (
        :platform_id,
        :platform_name,
        :platform_description
    )
    ";

        Action<DbCommand> action =
            cmd =>
            {
                cmd.AddParameterWithValue("platform_id", platform.PlatformId);
                cmd.AddParameterWithValue("platform_name", platform.PlatformName);
                cmd.AddParameterWithValue("platform_description", platform.PlatformDescription);
            };

        return ExecuteNonQuery(cmdText, action);
    }

    public int AddNewGameTransaction(GameTransactionDbItem gameTx)
    {
        string cmdText = $@"
    insert into game_transactions
    (
        transaction_id,
        purchase_date,
        is_virtual,
        store_id,
        platform_id,
        game_id,
        price,
        notes
    ) 
    values
    (
        :transaction_id,
        :purchase_date,
        :is_virtual,
        :store_id,
        :platform_id,
        :game_id,
        :price,
        :notes
    )
    ";

        Action<DbCommand> action =
            cmd =>
            {
                cmd.AddParameterWithValue("transaction_id", gameTx.TransactionId);
                cmd.AddParameterWithValue("purchase_date", gameTx.PurchaseDate);
                cmd.AddParameterWithValue("is_virtual", gameTx.IsVirtual);
                cmd.AddParameterWithValue("store_id", gameTx.StoreId);
                cmd.AddParameterWithValue("platform_id", gameTx.PlatformId);
                cmd.AddParameterWithValue("game_id", gameTx.GameId);
                cmd.AddParameterWithValue("price", gameTx.Price);
                cmd.AddParameterWithValue("notes", gameTx.Notes);
            };

        return ExecuteNonQuery(cmdText, action);
    }

    private int ExecuteNonQuery
    (
        string sqlText, 
        Action<DbCommand>? addParametersAction
    )
    {
        int affected = 0;

        Action<DbConnection> action =
            conn =>
            {
                string cmdText = sqlText;

                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.CommandType = System.Data.CommandType.Text;

                addParametersAction?.Invoke(cmd);

                affected = cmd.ExecuteNonQuery();
            };

        OpenAndExecute(action);

        return affected;
    }


    public GameDbItem[] GetAllGames() =>
        GetGamesByPartialName(null, null);

    public GameDbItem[] GetGamesByPartialName(string? partialName, string? partialTags)
    {
        List<GameDbItem> games = new List<GameDbItem>();

                string selectText = $@"
select 
    game_id, 
    game_name, 
    game_description,
    game_tags
from games
where 1 = 1 ";

                if (partialName is not null)
                {
                    selectText +=
                    $@"and game_name like '%' {_strConcatOperator} :partialname {_strConcatOperator} '%'";
                }

                if (partialTags is not null)
                {
                    selectText +=
                        $@"and game_tags like '%' {_strConcatOperator} :partialtags {_strConcatOperator} '%'";
                }

        Action<DbCommand> addParametersAction =
            cmd =>
            {
                if (partialName is not null)
                {
                    cmd.AddParameterWithValue("partialname", partialName);
                }

                if (partialTags is not null)
                {
                    cmd.AddParameterWithValue("partialtags", partialTags);
                }
            };

        Func<DbDataReader, GameDbItem> mapper =
            dataReader =>
                {
                    string id = dataReader.GetString(0);
                    string name = dataReader.GetString(1);
                    string description = dataReader.GetString(2);
                    string tags = dataReader.GetString(3);

                    GameDbItem game = new GameDbItem(id, name, description, tags);
                    return game;
                };

        return 
            GetItemsFromDb
            (
                selectText, 
                addParametersAction, 
                mapper
            );
    }

    public StoreDbItem[] GetAllStores() =>
        GetStoresByPartialName(null);

    public StoreDbItem[] GetStoresByPartialName(string? partialName)
    {
                string selectText = $@"
select 
    store_id, 
    store_name, 
    store_description,
    store_link
from stores
where 1 = 1 ";

        if (partialName is not null)
        {
            selectText +=
            $@"and store_name like '%' {_strConcatOperator} :partialname {_strConcatOperator} '%'";
        }

        Action<DbCommand> addParametersAction =
            cmd =>
            {
                if (partialName is not null)
                {
                    cmd.AddParameterWithValue("partialname", partialName);
                }
            };

        Func<DbDataReader, StoreDbItem> mapper =
            dataReader =>
            {
                string id = dataReader.GetString(0);
                string name = dataReader.GetString(1);
                string description = dataReader.GetString(2);
                string link = dataReader.GetString(3);

                StoreDbItem store = new StoreDbItem(id, name, description, link);
                return store;
            };

        return
            GetItemsFromDb
            (
                selectText,
                addParametersAction,
                mapper
            );
    }

    public PlatformDbItem[] GetAllPlatforms() =>
        GetPlatformsByPartialName(null);

    public PlatformDbItem[] GetPlatformsByPartialName(string? partialName)
    {
        string selectText = $@"
select 
    platform_id, 
    platform_name, 
    platform_description
from platforms
where 1 = 1 ";

        if (partialName is not null)
        {
            selectText += $@"and platform_name like '%' {_strConcatOperator} :partialname {_strConcatOperator} '%'";
        }

        Action<DbCommand> addParametersAction =
            cmd =>
            {
                if (partialName is not null)
                {
                    cmd.AddParameterWithValue("partialname", partialName);
                }
            };

        Func<DbDataReader, PlatformDbItem> mapper =
            dataReader =>
            {
                string id = dataReader.GetString(0);
                string name = dataReader.GetString(1);
                string description = dataReader.GetString(2);

                PlatformDbItem platform = new PlatformDbItem(id, name, description);
                return platform;
            };

        return
            GetItemsFromDb
            (
                selectText,
                addParametersAction,
                mapper
            );
    }

    private T[] GetItemsFromDb<T>
    (
        string selectText,
        Action<DbCommand>? addParametersAction,
        Func<DbDataReader, T> mapper
    )
    {
        List<T> items = new List<T>();

        Action<DbConnection> action =
            conn =>
            {       
                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = selectText;
                cmd.CommandType = System.Data.CommandType.Text;

                addParametersAction?.Invoke(cmd);

                using var dataReader = cmd.ExecuteReader();
                
                while (dataReader.Read())
                {
                    T item = mapper(dataReader);
                    items.Add(item);
                }
            };

        OpenAndExecute(action);
        return items.ToArray();
    }

}
