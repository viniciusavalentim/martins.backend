using Martins.Backend.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Martins.Backend.Domain.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public decimal Profit { get; set; }
        public decimal TotalCost { get; set; }
        public OrderStatusEnum Status { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public List<OrderAdditionalCost>? OrderAdditionalCosts { get; set; }
    }
}
