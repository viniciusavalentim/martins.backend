using Martins.Backend.Domain.Enums;
using MediatR;

namespace Martins.Backend.Domain.Commands.Expense.Update
{
    public class UpdateExpenseCommand : IRequest<UpdateExpenseCommandResponse>
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ExpenseCategoryEnum Category { get; set; }
        public decimal Amount { get; set; }
        public ExpenseTypeEnum Type { get; set; }
        public RecurrenceIntervalEnum? RecurrenceInterval { get; set; }
        public DateTime Date { get; set; }
        public string? Notes { get; set; }
    }
}
