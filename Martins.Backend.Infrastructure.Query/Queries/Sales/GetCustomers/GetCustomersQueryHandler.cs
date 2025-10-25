using Martins.Backend.Domain.Interfaces.Repositories.Sales;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Sales.GetCustomers
{
    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, GetCustomersQueryResponse>
    {
        private readonly ISaleRepositoryInterface _saleRepositoryInterface;

        public GetCustomersQueryHandler(ISaleRepositoryInterface saleRepositoryInterface)
        {
            _saleRepositoryInterface = saleRepositoryInterface;
        }

        public async Task<GetCustomersQueryResponse> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var response = new GetCustomersQueryResponse();
            var customers = await _saleRepositoryInterface.GetCustomers(request.SearchText);

            response.Data = customers.Data ?? [];

            return response;
        }
    }
}
