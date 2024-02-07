namespace GamesDataAccess;

using System.Data.Common;
using GamesDataAccess.DbItems;

partial class GamesDao
{

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


}
