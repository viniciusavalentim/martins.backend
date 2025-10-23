using Martins.Backend.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Martins.Backend.Domain.Models
{
    public class Material
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public decimal CurrentStock { get; set; }
        public UnitOfMeasureEnum UnitOfMeasure { get; set; }
        public decimal TotalCost { get; set; }
        public decimal UnitCost { get; set; }
        public decimal LowStockThreshold { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
