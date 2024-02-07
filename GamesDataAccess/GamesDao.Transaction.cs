using GamesDataAccess.DbItems;
using System.Data;
using System.Data.Common;

namespace GamesDataAccess;

partial class GamesDao
{
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
    notes varchar(2000) null,
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
				cmd.AddParameterWithValue("purchase_date", gameTx.PurchaseDate.ToDateTime(), DbType.DateTime);
				cmd.AddParameterWithValue("is_virtual", gameTx.IsVirtual, DbType.Boolean);
				cmd.AddParameterWithValue("store_id", gameTx.StoreId);
				cmd.AddParameterWithValue("platform_id", gameTx.PlatformId);
				cmd.AddParameterWithValue("game_id", gameTx.GameId);
				cmd.AddParameterWithValue("price", gameTx.Price);
				cmd.AddParameterWithValue("notes", gameTx.Notes);
			};

		return ExecuteNonQuery(cmdText, action);
	}

	public GameTransactionDbItem[] GetAllTransactions() =>
	   GetTransactionsById(null);

	public GameTransactionDbItem[] GetTransactionsById(string? id)
	{
		string selectText = $@"
     select
         transaction_id,
         purchase_date,
         is_virtual,
         store_id,
         game_id,
         platform_id,
         price,
         notes
     from game_transactions
     where 1 = 1 ";

		if (id is not null)
		{
			selectText +=
			$@"and transaction_id = :partialname";
		}

		Action<DbCommand> addParametersAction =
			cmd =>
			{
				if (id is not null)
				{
					cmd.AddParameterWithValue("partialname", id);
				}
			};

		Func<DbDataReader, GameTransactionDbItem> mapper =
			dataReader =>
			{
				string id = dataReader.GetString(0);
				var date = DateOnly.FromDateTime(dataReader.GetDateTime(1));
				bool isVirtual = dataReader.GetBoolean(2);
				string storeId = dataReader.GetString(3);
				string gameId = dataReader.GetString(4);
				string platformId = dataReader.GetString(5);
				decimal price = dataReader.GetDecimal(6);
				string notes = dataReader.GetString(7);

				var transaction = 
					new GameTransactionDbItem
					(
						id, 
						date, 
						isVirtual, 
						storeId, 
						gameId, 
						platformId, 
						price, 
						notes
					);
				return transaction;
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
