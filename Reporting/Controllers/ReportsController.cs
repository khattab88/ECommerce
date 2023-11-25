using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reporting.DataAccess;

namespace Reporting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMemoryReportStorage memoryReportStorage;

        public ReportsController(IMemoryReportStorage memoryReportStorage)
        {
            this.memoryReportStorage = memoryReportStorage;
        }

        // GET: api/<ReportsController>
        [HttpGet]
        public IEnumerable<Report> Get()
        {
            return memoryReportStorage.Get();
        }
    }
}
