using Martins.Backend.Domain.Models;

namespace Martins.Backend.Infrastructure.Query.Queries.Dashboard.Get
{
    public class DashboardDataQueryResponse
    {
        public DashboardModel Data { get; set; } = new DashboardModel();
    }
}
