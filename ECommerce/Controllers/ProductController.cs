using ECommerce.DataAccess;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plain.RabbitMQ;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductProvider _productProvider;
        private readonly IInventoryProvider _inventoryProvider;
        private readonly IPublisher publisher;

        public ProductController(IProductProvider productProvider, IInventoryProvider inventoryProvider, IPublisher publisher)
        {
            _productProvider = productProvider;
            _inventoryProvider = inventoryProvider;
            this.publisher = publisher;
        }

        
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _productProvider.Get();
        }

        [HttpPost]
        public async Task<Product> Post([FromBody] Product product)
        {
            Product created = await _productProvider.CreateProduct(product);

            await _inventoryProvider.CreateInventory(new Inventory() 
            {
                Name = product.ProductName,
                Quantity = 100,
                ProductId = created.Id,
            });

            publisher.Publish(JsonConvert.SerializeObject(product), "report.product", null);

            return created;
        }
    }
}
