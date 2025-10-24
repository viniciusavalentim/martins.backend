using Martins.Backend.Domain.Models;
using MediatR;

namespace Martins.Backend.Domain.Commands.Product.Update
{
    public class UpdateProductCommand : IRequest<UpdateProductCommandResponse>
    {
        public Guid ProductId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal ProfitMarginPorcent { get; set; }
        public decimal SellingPrice { get; set; }
        public required List<ProductMaterial> BillOfMaterials { get; set; }
        public List<ProductAdditionalCost>? AdditionalCosts { get; set; }
    }
}
