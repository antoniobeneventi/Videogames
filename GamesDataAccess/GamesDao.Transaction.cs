using GamesDataAccess.Criterias;
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

	public OwnedGameDbItem[] GetOwnedGamesByCriteria(GamesCriteria criteria)
	{
		string selectText = $@"
		 select
			 GT.transaction_id,
			 GT.purchase_date,
			 GT.is_virtual,
			 S.store_id,         
			 S.store_name,
			 S.store_description,
			 P.platform_id,
			 P.platform_name,
			 P.platform_description,
			 G.game_id,         
			 G.game_name,
			 G.game_description,
			 G.game_tags,
			 GT.price
		 from game_transactions GT
		 inner join stores S on GT.store_id = S.store_id
		 inner join platforms P on GT.platform_id = P.platform_id
		 inner join games G on GT.game_id = G.game_id
		 where 1 = 1 ";

        if (criteria?.PurchaseDateFrom is not null)
        {
            selectText +=
            $@"and GT.purchase_date >= :purchaseDateFrom";
        }

        if (criteria?.PurchaseDateTo is not null)
        {
            selectText +=
            $@"and GT.purchase_date <= :purchaseDateTo";
        }

        if (criteria?.IsVirtual is not null)
        {
            selectText +=
            $@"and GT.is_virtual = :isVirtual";
        }

        if (criteria?.PriceFrom is not null)
		{
			selectText +=
			$@"and GT.price >= :priceFrom";
		}

        if (criteria?.PriceTo is not null)
        {
            selectText +=
            $@"and GT.price <= :priceTo";
        }



        

        Action<DbCommand> addParametersAction =
			cmd =>
			{
				if (criteria?.PurchaseDateFrom is not null)
				{
					cmd.AddParameterWithValue("purchaseDateFrom", criteria.PurchaseDateFrom.Value.ToDateTime(), DbType.DateTime);
				}

                if (criteria?.PurchaseDateTo is not null)
                {
                    cmd.AddParameterWithValue("purchaseDateTo", criteria.PurchaseDateTo.Value.ToDateTime(), DbType.DateTime);
                }

                if (criteria?.IsVirtual is not null)
                {
                    cmd.AddParameterWithValue("isVirtual", criteria.IsVirtual.Value, DbType.Boolean);
                }

                if (criteria?.PriceFrom is not null)
                {
                    cmd.AddParameterWithValue("priceFrom", criteria.PriceFrom.Value, DbType.Decimal);
                }

                if (criteria?.PriceTo is not null)
                {
                    cmd.AddParameterWithValue("priceTo", criteria.PriceTo.Value, DbType.Decimal);
                }
            };

		Func<DbDataReader, OwnedGameDbItem> mapper =
			dataReader =>
			{
				string id = dataReader.GetString(0);
				var date = DateOnly.FromDateTime(dataReader.GetDateTime(1));
				bool isVirtual = dataReader.GetBoolean(2);
				
				string storeId = dataReader.GetString(3);
                string storeName = dataReader.GetString(4);
                string storeDescription = dataReader.GetString(5);

				string platformId = dataReader.GetString(6);
                string platformName = dataReader.GetString(7);
                string platformDescription = dataReader.GetString(8);

				string gameId = dataReader.GetString(9);
                string gameName = dataReader.GetString(10);
                string gameDescription = dataReader.GetString(11);
				string gameTags = dataReader.GetString(12);
				decimal price = dataReader.GetDecimal(13);

				var transaction =
					new OwnedGameDbItem
					(
						id,
						date,
						isVirtual,
						storeId,
						storeName,
						storeDescription,
						platformId,
						platformName,
						platformDescription,
						gameId,
						gameName,
						gameDescription,
						gameTags,
						price
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
