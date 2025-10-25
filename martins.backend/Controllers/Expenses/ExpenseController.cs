using Martins.Backend.Domain.Commands.Expense.Create;
using Martins.Backend.Domain.Commands.Expense.Update;
using Martins.Backend.Infrastructure.Query.Queries.Expenses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace martins.backend.Controllers.Expenses
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ExpenseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateExpenseCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSales([FromQuery] GetExpensesQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSale([FromBody] UpdateExpenseCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
