using Dapper;
using ECommerce.Models;
using System.Data;
using System.Data.SqlClient;

namespace ECommerce.DataAccess
{
    public interface IProductProvider
    {
        Product[] Get();
        Task<Product> CreateProduct(Product product);
    }

    public class ProductProvider : IProductProvider
    {
        private readonly string _connectionString;

        public ProductProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Product[] Get()
        {
            using var connection = new SqlConnection(_connectionString);
            return connection.Query<Product>(@"SELECT Id, Name, Description, Type FROM Product")
                .ToArray();
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var query = "INSERT INTO Product (Name, Description, Type) VALUES (@Name, @Description, @Type)" +
                "SELECT CAST(SCOPE_IDENTITY() AS int)";

            var parameters = new DynamicParameters();
            parameters.Add("Name", product.ProductName, DbType.String);
            parameters.Add("Description", product.Description, DbType.String);
            parameters.Add("Type", product.Type, DbType.String);

            using (var connection = new SqlConnection(_connectionString))
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);

                var created = new Product()
                {
                    Id = id,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    Type = product.Type,
                };

                return created;
            }
        }
    }
}
