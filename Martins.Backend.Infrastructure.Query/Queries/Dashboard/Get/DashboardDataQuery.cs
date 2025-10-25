using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Dashboard.Get
{
    public class DashboardDataQuery : IRequest<DashboardDataQueryResponse>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
