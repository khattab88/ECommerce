using ECommerce.DataAccess;
using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryProvider _inventoryProvider;

        public InventoryController(IInventoryProvider inventoryProvider)
        {
            _inventoryProvider = inventoryProvider;
        }

        
        [HttpGet]
        public IEnumerable<Inventory> Get()
        {
            return _inventoryProvider.Get();
        }
    }
}
