using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Text.Json.Nodes;
using tasklistDotNetReact.Services;

namespace tasklistDotNetReact.Controllers
{
  [Route("api/mocks")]
  [ApiController]
  public class MocksController : ControllerBase
  {

    private readonly ILogger<MocksController> _logger;
    private readonly MockService mockService;

    public MocksController(ILogger<MocksController> logger, MockService mockService)
    {
      _logger = logger;
      this.mockService = mockService;
    }

    [HttpGet("{type}")]
    public List<string> List(string type) => mockService.FindNames(type);

    [HttpGet("{type}/list")]
    public List<Dictionary<string, string>> ListDic(string type)
    {
      List<string> names = mockService.FindNames(type);
      List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
      foreach(string name in names)
      {
        Dictionary<string, string> dic = new Dictionary<string, string> { { "value", name }, { "label", name } };
        result.Add(dic);
      }
      return result;
    }

    [HttpPost("{type}/{name}")]
    public JsonNode Save(string type, string name, JsonNode template) => mockService.Save(type, name, template);

    [HttpGet("{type}/{name}")]
    public JsonNode Get(string type, string name) => mockService.FindByName(type, name);

    [HttpDelete("{type}/{name}")]
    public void Delete(string type, string name) => mockService.Delete(type, name);
  }
}