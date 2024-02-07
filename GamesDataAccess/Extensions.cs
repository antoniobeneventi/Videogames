namespace GamesDataAccess;

using System.Data;
// See https://aka.ms/new-console-template for more information
using System.Data.Common;

public static class Extensions
{
    public static void AddParameterWithValue
    (
        this DbCommand command,
        string paramName, 
        object value,
        DbType dbType = DbType.String
    )
    {
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = paramName;
        parameter.Value = value;
        parameter.DbType = dbType;

        command.Parameters.Add(parameter);
    }

    public static void SafeExecute(this Action action)
    {
        try
        {
            action();
        }
        catch (Exception ex)
        {
        }
    }

    public static DateTime ToDateTime(this DateOnly dt) =>
        new DateTime(dt.Year, dt.Month, dt.Day);
}
