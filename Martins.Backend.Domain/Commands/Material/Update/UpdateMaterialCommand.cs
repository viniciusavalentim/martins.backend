using Martins.Backend.Domain.Enums;
using MediatR;

namespace Martins.Backend.Domain.Commands.Material.Update
{
    public class UpdateMaterialCommand : IRequest<UpdateMaterialCommandResponse>
    {
        public required Guid MaterialId { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public decimal CurrentStock { get; set; }
        public UnitOfMeasureEnum UnitOfMeasure { get; set; }
        public decimal TotalCost { get; set; }
        public string? Supplier { get; set; }
    }
}
