using Martins.Backend.Domain.Commands.Sale.Create;
using Martins.Backend.Domain.Commands.Sale.CreateCustomer;
using Martins.Backend.Domain.Commands.Sale.UpdateStatus;
using Martins.Backend.Infrastructure.Query.Queries.Sales.Get;
using Martins.Backend.Infrastructure.Query.Queries.Sales.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace martins.backend.Controllers.Sale
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SaleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSales([FromQuery] GetSalesQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateSale([FromBody] UpdateStatusSaleCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("customer")]
        public async Task<IActionResult> GetCustomers([FromQuery] GetCustomersQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
