using Martins.Backend.Domain.Commands.Material;
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
        public async Task<IActionResult> CreateMaterial(CreateMaterialCommand request)
        {
            var result = await _mediator.Send(request);
            if (!result.Success) BadRequest(result);
            return Ok(result);
        }
    }
}
