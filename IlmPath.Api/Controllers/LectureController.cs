using Microsoft.AspNetCore.Mvc;

namespace IlmPath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LectureController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }
}
