namespace Martins.Backend.Domain.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
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
