using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using tasklistDotNetReact.Services;

namespace tasklistDotNetReact.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProcessController : ControllerBase
  {

    private readonly ILogger<ProcessController> _logger;
    private readonly ZeebeClientProvider _zeebeClientProvider;
    private readonly OperateService operateService;
    private readonly IConfiguration _configuration;

    public ProcessController(ILogger<ProcessController> logger, Services.ZeebeClientProvider zeebeClientProvider, OperateService operateService, IConfiguration configuration)
    {
      _logger = logger;
      _zeebeClientProvider = zeebeClientProvider;
      this.operateService = operateService;
      _configuration = configuration;
    }


    [HttpGet]
    public async Task<JsonResult> topology()
    {
      // Get cluster topology
      var topology = await _zeebeClientProvider.GetZeebeClient().TopologyRequest().Send();

      Console.WriteLine("Cluster connected: " + topology);

      return new JsonResult(new { topology = topology });
    }

    [HttpPost]
    public async Task<JsonResult> DeployProcess([FromQuery] string processName)
    {
      var processPath = _configuration.GetValue<string>("resourcePath");
      var demoProcessPath = $"{processPath}/{processName}.bpmn";

      var deployResponse = await _zeebeClientProvider.GetZeebeClient().NewDeployCommand()
          .AddResourceFile(demoProcessPath)
          .Send();

      var processDefinitionKey = deployResponse.Processes[0].ProcessDefinitionKey;

      return new JsonResult(new { processDefinitionKey = processDefinitionKey });
    }

    [HttpPost("{bpmnProcessId}/start")]
    public async Task<JsonResult> CreateProcessInstance(string bpmnProcessId, [FromBody] Dictionary<string, string> variables)
    {
      var processInstance = await _zeebeClientProvider.GetZeebeClient()
          .NewCreateProcessInstanceCommand()
          .BpmnProcessId(bpmnProcessId)
          .LatestVersion()
          .Variables(JsonConvert.SerializeObject(variables))
          .Send();


      //Refer below - setting new procecss variables
      await _zeebeClientProvider.GetZeebeClient().NewSetVariablesCommand(processInstance.ProcessInstanceKey).Variables("{\"email\":\"jothikiruthika.viswanathan@camunda.com\" }").Local().Send();

      return new JsonResult(new { ProcessInstanceKey = processInstance.ProcessInstanceKey });
    }

    [HttpGet("definition/latest")]
    public async Task<JsonResult> latestDefinitions()
    {
      return new JsonResult(await operateService.ProcessDefinitions());
    }
  }
}