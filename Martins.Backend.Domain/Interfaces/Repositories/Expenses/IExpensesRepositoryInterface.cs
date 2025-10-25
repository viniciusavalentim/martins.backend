using Martins.Backend.Domain.Commands.Expense.Create;
using Martins.Backend.Domain.Commands.Expense.Update;
using Martins.Backend.Domain.Enums;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Repositories.Response;

namespace Martins.Backend.Domain.Interfaces.Repositories.Expenses
{
    public interface IExpensesRepositoryInterface
    {
        Task<RepositoryResponseBase<OperationalExpense>> CreateExpense(CreateExpenseCommand request);
        Task<RepositoryResponseBase<bool>> UpdateExpense(UpdateExpenseCommand request);
        Task<RepositoryResponseBase<List<OperationalExpense>>> GetExpenses(
                string? searchText,
                ExpenseCategoryEnum? category,
                DateTime? startDate,
                DateTime? endDate);
    }
}
