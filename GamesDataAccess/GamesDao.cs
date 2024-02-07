namespace GamesDataAccess;

using System.Data.Common;
using GamesDataAccess.DbItems;

public partial class GamesDao
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
}
