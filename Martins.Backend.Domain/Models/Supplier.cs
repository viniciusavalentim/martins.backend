using System.ComponentModel.DataAnnotations;

namespace Martins.Backend.Domain.Models
{
    public class Supplier
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? ContactName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
