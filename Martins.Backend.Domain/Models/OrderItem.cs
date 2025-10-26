using System.Text.Json.Serialization;

namespace Martins.Backend.Domain.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        [JsonIgnore]
        public Guid OrderId { get; set; }
        [JsonIgnore]
        public Order? Order { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public double Quantity { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal ExpectedProfit { get; set; }
        public decimal RealProfit { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
    }
}
