namespace Martins.Backend.Domain.Models
{
    public class ProductMaterial
    {
        public Guid Id { get; set; }
        public decimal QuantityUsed { get; set; }

        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        public Guid MaterialId { get; set; }
        public Material? Material { get; set; }
    }
}
