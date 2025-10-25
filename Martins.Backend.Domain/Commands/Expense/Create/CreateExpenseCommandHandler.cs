using Martins.Backend.Domain.Interfaces.Repositories.Expenses;
using MediatR;

namespace Martins.Backend.Domain.Commands.Expense.Create
{
    public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, CreateExpenseCommandResponse>
    {
        private readonly IExpensesRepositoryInterface _expensesRepositoryInterface;

        public CreateExpenseCommandHandler(IExpensesRepositoryInterface expensesRepositoryInterface)
        {
            _expensesRepositoryInterface = expensesRepositoryInterface;
        }

        public async Task<CreateExpenseCommandResponse> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateExpenseCommandResponse();
            var updateExpense = await _expensesRepositoryInterface.CreateExpense(request);

            response.Success = updateExpense.Success;
            response.Message = updateExpense.Message;

            return response;
        }
    }
}
