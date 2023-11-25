using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models
{
    public class InventoryResponse
    {
        public int OrderId { get; set; }
        public bool IsSuccess { get; set; }
    }
}
