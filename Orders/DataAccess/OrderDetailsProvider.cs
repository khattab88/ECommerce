using Dapper;
using ECommerce.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Orders.DataAccess
{
    public interface IOrderDetailsProvider
    {
        OrderDetail[] Get();
    }

    public class OrderDetailsProvider : IOrderDetailsProvider
    {
        private readonly string _connectionString;

        public OrderDetailsProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public OrderDetail[] Get()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<OrderDetail>(@"SELECT o.UserName AS [User], od.ProductName AS Name, od.Quantity  FROM [Order] o
                                            JOIN [OrderDetail] od on o.Id = od.OrderId")
                .ToArray();
        }
    }
}
