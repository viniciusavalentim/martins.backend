using Martins.Backend.Domain.Models;
using MediatR;

namespace Martins.Backend.Domain.Commands.Product.Create
{
    public class CreateProductCommand : IRequest<CreateProductCommandResponse>
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public double ProfitMarginPorcent { get; set; }
        public decimal SellingPrice { get; set; } = 0;
        public required List<ProductMaterial> BillOfMaterials { get; set; }
        public List<ProductAdditionalCost>? AdditionalCosts { get; set; }
    }
}
