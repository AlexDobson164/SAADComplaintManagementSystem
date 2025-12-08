using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("AuthToken", Name = "GetAuthToken"), BasicAuth]
    public IActionResult Token()
    {        
        return Ok();
    }
}
