using Martins.Backend.Domain.Commands.Material.AddStock;
using Martins.Backend.Domain.Commands.Material.Create;
using Martins.Backend.Domain.Commands.Material.Update;
using Martins.Backend.Infrastructure.Query.Queries.Material.GetMaterials;
using Martins.Backend.Infrastructure.Query.Queries.Materials.GetReportMaterials;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace martins.backend.Controllers.Material
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-material")]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterials([FromQuery] GetMaterialsQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReportMaterials([FromQuery] GetReportMaterialQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("add-stock")]
        public async Task<IActionResult> AddStockMaterial([FromBody] AddStockCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMaterial([FromBody] UpdateMaterialCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
