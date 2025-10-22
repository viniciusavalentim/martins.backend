using Martins.Backend.Domain.Enums;

namespace Martins.Backend.Domain.Models
{
    public class OperationalExpense
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ExpenseCategoryEnum Category { get; set; }
        public decimal Amount { get; set; }
        public ExpenseTypeEnum Type { get; set; }
        public RecurrenceIntervalEnum? RecurrenceInterval { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
