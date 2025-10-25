using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Sales.GetCustomers
{
    public class GetCustomersQuery : IRequest<GetCustomersQueryResponse>
    {
        public string? SearchText { get; set; }
    }
}
