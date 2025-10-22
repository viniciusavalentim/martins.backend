namespace Martins.Backend.Domain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalAdditionalCosts { get; set; }
        public double StockQuantity { get; set; }
        public decimal Profit { get; set; }
        public double ProfitMarginPorcent { get; set; }
        public double StockOnHand { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public required List<ProductMaterial> BillOfMaterials { get; set; }
        public List<ProductAdditionalCost>? AdditionalCosts { get; set; }
    }
}
