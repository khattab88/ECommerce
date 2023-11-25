using Dapper;
using ECommerce.Models;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace ECommerce.DataAccess
{
    public interface IInventoryProvider
    {
        Inventory[] Get();
        Task CreateInventory(Inventory inventory);
    }

    public class InventoryProvider : IInventoryProvider
    {
        private readonly string _connectionString;

        public InventoryProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Inventory[] Get()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<Inventory>(@"SELECT Id, Name, Quantity, ProductId FROM Inventory")
                .ToArray();
        }

        public async Task CreateInventory(Inventory inventory)
        {
            var query = "INSERT INTO Inventory (Name, Quantity, ProductId) VALUES (@Name, @Quantity, @ProductId)" +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", inventory.Name, DbType.String);
            parameters.Add("Quantity", inventory.Quantity, DbType.Int32);
            parameters.Add("ProductId", inventory.ProductId, DbType.Int32);

            using (var connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
            }
        }
    }
}
