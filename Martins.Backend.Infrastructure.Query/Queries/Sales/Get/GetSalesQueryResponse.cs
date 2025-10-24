using Martins.Backend.Domain.Models;

namespace Martins.Backend.Infrastructure.Query.Queries.Sales.Get
{
    public class GetSalesQueryResponse
    {
        public List<Order> Data { get; set; } = [];
    }
}
