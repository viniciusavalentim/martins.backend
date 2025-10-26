using Martins.Backend.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Martins.Backend.Domain.Models
{
    public class ProductAdditionalCost
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public CostTypeEnum Type { get; set; }
        public decimal Value { get; set; }

        [JsonIgnore]
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
    }
}
