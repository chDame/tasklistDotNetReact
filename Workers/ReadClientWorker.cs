using System.Text.Json.Nodes;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;

namespace tasklistDotNetReact.Services
{
  [JobType("readClientData")]
  public class ReadClientWorker : IAsyncZeebeWorkerWithResult<JsonNode>
  {
    private readonly MockService mockService;
    public ReadClientWorker(MockService mockService)
    {
      this.mockService = mockService;
    }
    public async Task<JsonNode> HandleJob(ZeebeJob job, CancellationToken cancellationToken)
    {
      JsonNode variables = job.getVariables<JsonNode>();
      return mockService.FindByName("clients", variables["client"].ToString());
    }
  }
}

