using System.Text.Json.Serialization;

namespace Martins.Backend.Domain.Models
{
    public class ProductMaterial
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public decimal QuantityUsed { get; set; }

        [JsonIgnore]
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }

        public Guid MaterialId { get; set; }
        public Material? Material { get; set; }
    }
}
