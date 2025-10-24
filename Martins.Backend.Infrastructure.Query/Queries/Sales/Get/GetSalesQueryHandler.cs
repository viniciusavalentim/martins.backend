using Martins.Backend.Domain.Interfaces.Repositories.Sales;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Sales.Get
{
    public class GetSalesQueryHandler : IRequestHandler<GetSalesQuery, GetSalesQueryResponse>
    {
        private readonly ISaleRepositoryInterface _saleRepositoryInterface;
        public GetSalesQueryHandler(ISaleRepositoryInterface saleRepositoryInterface)
        {
            _saleRepositoryInterface = saleRepositoryInterface;
        }

        public async Task<GetSalesQueryResponse> Handle(GetSalesQuery request, CancellationToken cancellationToken)
        {
            var response = new GetSalesQueryResponse();
            var sales = await _saleRepositoryInterface.GetSales(request.CustomerId, request.Status, request.StartDate, request.EndDate);

            response.Data = sales.Data ?? [];

            return response;
        }
    }
}
