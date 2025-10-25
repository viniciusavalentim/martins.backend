using Martins.Backend.Domain.Interfaces.Repositories.Sales;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Dashboard.Get
{
    public class DashboardDataQueryHandler : IRequestHandler<DashboardDataQuery, DashboardDataQueryResponse>
    {
        private readonly ISaleRepositoryInterface _saleRepositoryInterface;
        public DashboardDataQueryHandler(ISaleRepositoryInterface saleRepositoryInterface)
        {
            _saleRepositoryInterface = saleRepositoryInterface;
        }

        public async Task<DashboardDataQueryResponse> Handle(DashboardDataQuery request, CancellationToken cancellationToken)
        {
            var response = new DashboardDataQueryResponse();
            var dashboard = await _saleRepositoryInterface.GetDashboardData(request.StartDate, request.EndDate);
            response.Data = dashboard.Data;

            return response;
        }
    }
}
