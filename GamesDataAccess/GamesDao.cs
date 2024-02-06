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

    private void ExecuteNonQuery(string commandText)
    {
        Action<DbConnection> action =
            conn =>
            {                
                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = commandText;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
            };

        OpenAndExecute(action);
    }


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

    public int AddNewGame(Game game)
    {
        int affected = 0;

        Action<DbConnection>  action = 
            conn =>
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

                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.CommandType = System.Data.CommandType.Text;

                cmd.AddParameterWithValue("game_id", game.GameId);
                cmd.AddParameterWithValue("game_name", game.GameName);
                cmd.AddParameterWithValue("game_description", game.GameDescription);
                cmd.AddParameterWithValue("game_tags", game.GameTags);


                affected = cmd.ExecuteNonQuery();
            };

        OpenAndExecute(action);

        return affected;
    }

    public Game[] GetAllGames() =>
        GetGamesByPartialName(null, null);

    public Game[] GetGamesByPartialName(string? partialName, string? partialTags)
    {
        List<Game> games = new List<Game>();

        Action<DbConnection> action =
            conn =>
            {
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

                using DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = selectText;
                cmd.CommandType = System.Data.CommandType.Text;

                if (partialName is not null)
                {
                    cmd.AddParameterWithValue("partialname", partialName);
                }

                if (partialTags is not null)
                {
                    cmd.AddParameterWithValue("partialtags", partialTags);
                }

                using var dataReader = cmd.ExecuteReader();
                
                while (dataReader.Read())
                {
                    string id = dataReader.GetString(0);
                    string name = dataReader.GetString(1);
                    string description = dataReader.GetString(2);
                    string tags = dataReader.GetString(3);

                    Game game = new Game(id, name, description, tags);
                    games.Add(game);
                }
            };

        OpenAndExecute(action);
        return games.ToArray();
    }
}
