using Martins.Backend.Domain.Interfaces.Repositories.Expenses;
using MediatR;

namespace Martins.Backend.Infrastructure.Query.Queries.Expenses
{
    public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, GetExpensesQueryResponse>
    {
        private readonly IExpensesRepositoryInterface _expensesRepositoryInterface;

        public GetExpensesQueryHandler(IExpensesRepositoryInterface expensesRepositoryInterface)
        {
            _expensesRepositoryInterface = expensesRepositoryInterface;
        }

        public async Task<GetExpensesQueryResponse> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
        {
            var response = new GetExpensesQueryResponse();
            var getExpenses = await _expensesRepositoryInterface.GetExpenses(request.SearchText, request.Category, request.StartDate, request.EndDate);

            response.Data = getExpenses.Data ?? [];
            return response;
        }
    }
}
