using Martins.Backend.Domain.Enums;

namespace Martins.Backend.Domain.Models
{
    public class ProductAdditionalCost
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public CostTypeEnum Type { get; set; }
        public decimal Value { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
