using Martins.Backend.Domain.Enums;

namespace Martins.Backend.Domain.Models.Report
{
    public class ReportMaterial
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Category { get; set; }
        public double CurrentStock { get; set; }
        public UnitOfMeasureEnum UnitOfMeasure { get; set; }
        public decimal TotalCost { get; set; }
        public decimal UnitCost { get; set; }
        public double? LowStockThreshold { get; set; }
        public MovementTypeEnum MovementType { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
