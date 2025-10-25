namespace Martins.Backend.Domain.Commands.Expense.Update
{
    public class UpdateExpenseCommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
