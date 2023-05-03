using Microsoft.AspNetCore.Mvc;

namespace tasklistDotNetReact.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {

    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
      _logger = logger;
    }

    [HttpGet("logout")]
    public Auth Logout()
    {
      Auth auth = new Auth();
      auth.result = "done";
      return auth;
    }

    [HttpPost("login")]
    public Auth Login([FromBody] Dictionary<string, string> credentials)
    {
      Auth auth = new Auth();
      auth.username = credentials.GetValueOrDefault("username");
      auth.profile = "Admin";
      auth.token = "someToken";
      auth.groups = new List<string>{"group1", "group2" };
      return auth;
    }
  }
}