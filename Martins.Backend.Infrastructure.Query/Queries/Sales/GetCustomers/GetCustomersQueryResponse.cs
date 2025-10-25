using Martins.Backend.Domain.Models;

namespace Martins.Backend.Infrastructure.Query.Queries.Sales.GetCustomers
{
    public class GetCustomersQueryResponse
    {
        public List<Customer> Data { get; set; } = [];
    }
}
