namespace GamesDataAccess;

using System.Data.Common;
using GamesDataAccess.DbItems;

partial class GamesDao
{
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
        {_parameterPrefix}store_id,
        {_parameterPrefix}store_name,
        {_parameterPrefix}store_description,
        {_parameterPrefix}store_link
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
            selectText += $@"and store_name like '%' {_strConcatOperator} {_parameterPrefix}partialname {_strConcatOperator} '%'";
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
}
