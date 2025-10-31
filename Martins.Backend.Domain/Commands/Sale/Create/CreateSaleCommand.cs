using Martins.Backend.Domain.Enums;
using Martins.Backend.Domain.Models;
using MediatR;

namespace Martins.Backend.Domain.Commands.Sale.Create
{
    public class CreateSaleCommand : IRequest<CreateSaleCommandResponse>
    {
        public Guid? CustomerId { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public List<OrderItem> OrderItems { get; set; } = [];
        public List<OrderAdditionalCost>? AdditionalCosts { get; set; }
        public string Observations { get; set; } = string.Empty;
    }
}
