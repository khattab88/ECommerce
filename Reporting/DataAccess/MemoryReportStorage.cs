using ECommerce.Models;

namespace Reporting.DataAccess
{
    public interface IMemoryReportStorage
    {
        void Add(Report report);
        IEnumerable<Report> Get();
    }

    public class MemoryReportStorage : IMemoryReportStorage
    {
        private readonly IList<Report> reports = new List<Report>();

        public void Add(Report report)
        {
            reports.Add(report);
        }

        public IEnumerable<Report> Get()
        {
            return reports;
        }
    }
}
