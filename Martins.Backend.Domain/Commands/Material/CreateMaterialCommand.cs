using Martins.Backend.Domain.Enums;
using MediatR;

namespace Martins.Backend.Domain.Commands.Material
{
    public class CreateMaterialCommand : IRequest<CreateMaterialCommandResponse>
    {
        public required string Name { get; set; }
        public required string Category { get; set; }
        public decimal CurrentStock { get; set; }
        public UnitOfMeasureEnum UnitOfMeasure { get; set; }
        public decimal TotalCost { get; set; }
        public string? Supplier { get; set; }
    }
}
