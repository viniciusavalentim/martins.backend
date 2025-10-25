using Martins.Backend.Domain.Enums;
using MediatR;

namespace Martins.Backend.Domain.Commands.Expense.Create
{
    public class CreateExpenseCommand : IRequest<CreateExpenseCommandResponse>
    {
        public required string Name { get; set; }
        public ExpenseCategoryEnum Category { get; set; }
        public decimal Amount { get; set; }
        public ExpenseTypeEnum Type { get; set; }
        public RecurrenceIntervalEnum? RecurrenceInterval { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
    }
}
