using Martins.Backend.Infrastructure.Query.Queries.Dashboard.Get;
using Martins.Backend.Infrastructure.Query.Queries.Expenses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace martins.backend.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData([FromQuery] DashboardDataQuery request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
