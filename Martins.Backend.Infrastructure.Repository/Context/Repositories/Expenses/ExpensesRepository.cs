using Martins.Backend.Domain.Commands.Expense.Create;
using Martins.Backend.Domain.Commands.Expense.Update;
using Martins.Backend.Domain.Enums;
using Martins.Backend.Domain.Interfaces.Repositories.Expenses;
using Martins.Backend.Domain.Models;
using Martins.Backend.Domain.Models.Repositories.Response;
using Martins.Backend.Infrastructure.Query.Queries.Expenses;
using Microsoft.EntityFrameworkCore;

namespace Martins.Backend.Infrastructure.Repository.Context.Repositories.Expenses
{
    public class ExpensesRepository : IExpensesRepositoryInterface
    {
        private readonly ApplicationDbContext _context;

        public ExpensesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RepositoryResponseBase<OperationalExpense>> CreateExpense(CreateExpenseCommand request)
        {
            var response = new RepositoryResponseBase<OperationalExpense>();

            try
            {
                if (request.Type == ExpenseTypeEnum.Recurring && !request.RecurrenceInterval.HasValue)
                {
                    response.Success = false;
                    response.Message = "Despesas recorrentes devem ter um intervalo de recorrência (Diário, Semanal, etc.).";
                    return response;
                }

                var expense = new OperationalExpense
                {
                    Name = request.Name,
                    Category = request.Category,
                    Amount = request.Amount,
                    Type = request.Type,
                    RecurrenceInterval = request.Type == ExpenseTypeEnum.OneTime ? null : request.RecurrenceInterval,
                    Date = request.Date,
                    Notes = request.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.OperationalExpense.AddAsync(expense);
                await _context.SaveChangesAsync();

                response.Data = expense;
                response.Success = true;
                response.Message = "Despesa criada com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao criar despesa: {ex.Message}";
                return response;
            }
        }

        public async Task<RepositoryResponseBase<bool>> UpdateExpense(UpdateExpenseCommand request)
        {
            var response = new RepositoryResponseBase<bool>();

            try
            {
                if (request.Type == ExpenseTypeEnum.Recurring && !request.RecurrenceInterval.HasValue)
                {
                    response.Success = false;
                    response.Message = "Despesas recorrentes devem ter um intervalo de recorrência (Diário, Semanal, etc.).";
                    return response;
                }

                var expense = await _context.OperationalExpense.FindAsync(request.Id);

                if (expense == null)
                {
                    response.Success = false;
                    response.Message = "Despesa não encontrada.";
                    return response;
                }

                expense.Name = request.Name;
                expense.Category = request.Category;
                expense.Amount = request.Amount;
                expense.Type = request.Type;
                expense.RecurrenceInterval = request.Type == ExpenseTypeEnum.OneTime ? null : request.RecurrenceInterval;
                expense.Date = request.Date;
                expense.Notes = request.Notes;

                await _context.SaveChangesAsync();

                response.Success = true;
                response.Message = "Despesa atualizada com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao atualizar despesa: {ex.Message}";
                return response;
            }
        }

        public async Task<RepositoryResponseBase<List<OperationalExpense>>> GetExpenses(
                string? searchText,
                ExpenseCategoryEnum? category,
                DateTime? startDate,
                DateTime? endDate
        )
        {
            var response = new RepositoryResponseBase<List<OperationalExpense>>();

            try
            {
                IQueryable<OperationalExpense> queryable = _context.OperationalExpense;

                if (category.HasValue)
                {
                    queryable = queryable.Where(e => e.Category == category.Value);
                }

                if (startDate.HasValue)
                {
                    queryable = queryable.Where(e => e.Date.Date >= startDate.Value.Date);
                }

                if (endDate.HasValue)
                {
                    var inclusiveEndDate = endDate.Value.Date.AddDays(1);
                    queryable = queryable.Where(e => e.Date < inclusiveEndDate);
                }

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    var searchTerm = searchText.Trim().ToLower();
                    queryable = queryable.Where(e => e.Name.ToLower().Contains(searchTerm) ||
                                                     e.Notes != null && e.Notes.ToLower().Contains(searchTerm));
                }

                var expenses = await queryable
                                     .OrderByDescending(e => e.Date)
                                     .ToListAsync();

                response.Data = expenses;
                response.Success = true;
                response.Message = "Despesas recuperadas com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Erro ao buscar despesas: {ex.Message}";
                return response;
            }
        }
    }
}
