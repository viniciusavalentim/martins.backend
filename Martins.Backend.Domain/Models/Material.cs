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
        public int CurrentStock { get; set; }
        public UnitOfMeasureEnum UnitOfMeasure { get; set; }
        public int TotalCost { get; set; }
        public int UnitCost { get; set; }
        public int LowStockThreshold { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}
