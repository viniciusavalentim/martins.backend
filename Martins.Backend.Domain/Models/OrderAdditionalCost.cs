using System.Text.Json.Serialization;

namespace Martins.Backend.Domain.Models;

public class OrderAdditionalCost
{
    [JsonIgnore]
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid OrderId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public Guid? OrderItemId { get; set; }
    public OrderAdditionalCostCategory? Category { get; set; }
    public int? Quantity { get; set; }
}

public enum OrderAdditionalCostCategory
{
    Shipping = 1,
    Packaging = 2,
    Delivery = 3,
    Custom = 4,
    Other = 5
}
