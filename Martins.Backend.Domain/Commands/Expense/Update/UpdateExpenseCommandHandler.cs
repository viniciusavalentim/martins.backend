using Martins.Backend.Domain.Interfaces.Repositories.Expenses;
using MediatR;

namespace Martins.Backend.Domain.Commands.Expense.Update
{
    public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, UpdateExpenseCommandResponse>
    {
        private readonly IExpensesRepositoryInterface _expensesRepositoryInterface;

        public UpdateExpenseCommandHandler(IExpensesRepositoryInterface expensesRepositoryInterface)
        {
            _expensesRepositoryInterface = expensesRepositoryInterface;
        }

        public async Task<UpdateExpenseCommandResponse> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdateExpenseCommandResponse();
            var updateExpense = await _expensesRepositoryInterface.UpdateExpense(request);

            response.Success = updateExpense.Success;
            response.Message = updateExpense.Message;

            return response;
        }
    }
}
