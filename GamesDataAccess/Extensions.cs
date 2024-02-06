namespace GamesDataAccess;
// See https://aka.ms/new-console-template for more information
using System.Data.Common;

public static class Extensions
{
    public static void AddParameterWithValue
    (
        this DbCommand command,
        string paramName, 
        object value
    )
    {
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = paramName;
        parameter.Value = value;

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
}
