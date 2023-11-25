using Dapper;
using System.Data.SqlClient;

namespace Orders.DataAccess
{
    public interface IOrderDeletor
    {
        Task Delete(int orderId);
    }

    public class OrderDeletor : IOrderDeletor
    {
        private readonly string connectionString;

        public OrderDeletor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task Delete(int orderId)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            try
            {
                await connection.ExecuteAsync("DELETE FROM OrderDetail WHERE OrderId = @orderId", new { orderId }, transaction: transaction);
                await connection.ExecuteAsync("DELETE FROM [Order] WHERE Id = @orderId", new { orderId }, transaction: transaction);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
        }
    }
}
