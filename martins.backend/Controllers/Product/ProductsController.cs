using Martins.Backend.Domain.Commands.Product.Create;
using Martins.Backend.Domain.Commands.Product.Produce;
using Martins.Backend.Domain.Commands.Product.Update;
using Martins.Backend.Infrastructure.Query.Queries.Products.GetProducts;
using Martins.Backend.Infrastructure.Query.Queries.Products.GetReportProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace martins.backend.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReportProducts([FromQuery] GetReportProductsQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut("produce")]
        public async Task<IActionResult> ProduceProduct([FromBody] ProduceProductCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
