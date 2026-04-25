using Microsoft.AspNetCore.Mvc;

namespace OCR_UI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "healthy", timestamp = DateTime.UtcNow });

    [HttpGet("version")]
    public IActionResult Version() => Ok(new { version = "1.0.0", name = "OCR-UI" });
}
