using Martins.Backend.Domain.Enums;

namespace Martins.Backend.Domain.Models.Report
{
    public class ReportProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalAdditionalCosts { get; set; }
        public double StockQuantity { get; set; }
        public decimal Profit { get; set; }
        public double ProfitMarginPorcent { get; set; }
        public double StockOnHand { get; set; }
        public MovementTypeProductEnum MovementType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<ProductMaterial> BillOfMaterials { get; set; } = new List<ProductMaterial>();
        public List<ProductAdditionalCost>? AdditionalCosts { get; set; }
    }
}
