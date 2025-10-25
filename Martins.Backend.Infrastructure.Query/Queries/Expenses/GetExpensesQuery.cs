using Martins.Backend.Domain.Enums;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Expenses
{
    public class GetExpensesQuery : IRequest<GetExpensesQueryResponse>
    {
        public ExpenseCategoryEnum? Category { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SearchText { get; set; }
    }
}
