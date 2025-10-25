using Martins.Backend.Domain.Enums;
using MediatR;

namespace Martins.Backend.Domain.Commands.Sale.UpdateStatus
{
    public class UpdateStatusSaleCommand : IRequest<UpdateStatusSaleCommandResponse>
    {
        public Guid OrderId { get; set; }
        public OrderStatusEnum Status { get; set; }
    }
}
