using Martins.Backend.Domain.Models;

namespace Martins.Backend.Infrastructure.Query.Queries.Expenses
{
    public class GetExpensesQueryResponse
    {
        public List<OperationalExpense> Data { get; set; } = [];
    }
}
