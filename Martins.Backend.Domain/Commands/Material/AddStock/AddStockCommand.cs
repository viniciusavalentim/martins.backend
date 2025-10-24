using MediatR;

namespace Martins.Backend.Domain.Commands.Material.AddStock
{
    public class AddStockCommand : IRequest<AddStockCommandResponse>
    {
        public Guid MaterialId { get; set; }
        public decimal QuantityToAdd { get; set; }
        public decimal totalCost { get; set; }
        public string? Supplier { get; set; }
    }
}
