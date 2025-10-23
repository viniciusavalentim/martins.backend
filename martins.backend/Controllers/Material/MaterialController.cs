using Martins.Backend.Domain.Commands.Material;
using Martins.Backend.Infrastructure.Query.Queries.Material.GetMaterials;
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

        [HttpPost("Create-material")]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMAterials([FromQuery] GetMaterialsQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
