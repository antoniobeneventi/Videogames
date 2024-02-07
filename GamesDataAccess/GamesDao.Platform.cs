using GamesDataAccess.DbItems;
using System.Data.Common;

namespace GamesDataAccess;

/// <summary>
/// Methods for managing platforms table
/// </summary>
partial class GamesDao
{

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
        {_parameterPrefix}platform_id,
        {_parameterPrefix}platform_name,
        {_parameterPrefix}platform_description
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
            selectText += $@"and platform_name like '%' {_strConcatOperator} {_parameterPrefix}partialname {_strConcatOperator} '%'";
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

}
