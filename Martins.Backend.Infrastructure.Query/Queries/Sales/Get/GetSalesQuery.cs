using Martins.Backend.Domain.Enums;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Sales.Get
{
    public class GetSalesQuery : IRequest<GetSalesQueryResponse>
    {
        public Guid? CustomerId { get; set; }
        public OrderStatusEnum? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
