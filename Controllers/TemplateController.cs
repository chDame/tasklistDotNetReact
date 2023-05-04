using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Text.Json.Nodes;
using tasklistDotNetReact.Services;

namespace tasklistDotNetReact.Controllers
{
  [Route("api/elttemplates")]
  [ApiController]
  public class TemplatesController : ControllerBase
  {

    private readonly ILogger<TemplatesController> _logger;
    private readonly TemplateService templateService;

    public TemplatesController(ILogger<TemplatesController> logger, TemplateService templateService)
    {
      _logger = logger;
      this.templateService = templateService;
    }

    [HttpGet]
    public List<string> List() => templateService.FindNames();

    [HttpPost("{name}")]
    public JsonNode Save(string name, JsonNode template) => templateService.Save(name, template);

    [HttpGet("{name}")]
    public JsonNode Get(string name) => templateService.FindByName(name);

    [HttpDelete("{name}")]
    public void Delete(string name) => templateService.Delete(name);


    [HttpGet("new/{name}")]
    public JsonNode NewTemplate(string name)
    {
      JsonNode content = JsonNode.Parse(@"{
""$schema"" : ""https://unpkg.com/@camunda/zeebe-element-templates-json-schema/resources/schema.json"",
  ""name"" : ""Some Connector"",
  ""id"" : ""io.camunda.someconnector"",
  ""description"" : ""Execute GraphQL query"",
  ""version"" : 1,
  ""category"" : {
          ""id"" : ""connectors"",
    ""name"" : ""Connectors""
  },
  ""appliesTo"" : [ ""bpmn:Task"" ],
  ""elementType"" : {
          ""value"" : ""bpmn:ServiceTask""
  },
  ""groups"" : [ {
          ""id"" : ""group1"",
    ""label"" : ""group1""
  }],
  ""properties"" : [ {
          ""type"" : ""Hidden"",
    ""value"" : ""io.camunda:some-connector:1"",
    ""binding"" : {
            ""type"" : ""zeebe:taskDefinition:type""
    }
        }, {
          ""label"" : ""Property"",
    ""id"" : ""myProperty"",
    ""group"" : ""group1"",
    ""description"" : ""some description of the propertu"",
    ""value"" : ""a value"",
    ""type"" : ""Text"",
    ""binding"" : {
            ""type"" : ""zeebe:input"",
      ""name"" : ""myProperty""
    }
        }]
}");
      return templateService.Save(name, content);
    }
  }
}