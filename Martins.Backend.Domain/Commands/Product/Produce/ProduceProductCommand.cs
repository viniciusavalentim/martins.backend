using MediatR;

namespace Martins.Backend.Domain.Commands.Product.Produce
{
    public class ProduceProductCommand : IRequest<ProduceProductCommandResponse>
    {
        public Guid ProductId { get; set; }
        public decimal QuantityToProduce { get; set; }
        public string Observation { get; set; } = string.Empty;
    }
}
