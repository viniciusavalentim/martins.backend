using Microsoft.AspNetCore.Mvc;

namespace martins.backend.Controllers.Material
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        [HttpPost("Create-material")]
        public IActionResult CreateMaterial(Martins.Backend.Domain.Models.Material request)
        {
            return Ok("Material created successfully.");
        }
    }
}
