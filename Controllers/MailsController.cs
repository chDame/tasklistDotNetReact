using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.Json.Nodes;
using tasklistDotNetReact.Services;

namespace tasklistDotNetReact.Controllers
{
  [Route("api/edition/[controller]")]
  [ApiController]
  public class MailsController : ControllerBase
  {

    private readonly ILogger<MailsController> _logger;
    private readonly MailService mailService;

    public MailsController(ILogger<MailsController> logger, MailService mailService)
    {
      _logger = logger;
      this.mailService = mailService;
    }

    [HttpGet("names")]
    public List<string> List() => mailService.FindNames();

    [HttpPost]
    public JsonNode Save(JsonNode mail) => mailService.Save(mail);

    [HttpGet("{name}")]
    public JsonNode Get(string name) => mailService.FindByName(name);

    [HttpDelete("{name}")]
    public void Delete(string name) => mailService.Delete(name);


    [HttpPost("preview")]
    public JsonNode Preview(JsonNode mail) => mailService.Preview(mail);
  }
}