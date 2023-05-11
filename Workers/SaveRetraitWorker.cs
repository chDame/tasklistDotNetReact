using System.Text.Json.Nodes;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;

namespace tasklistDotNetReact.Services
{
  [JobType("saisieRetrait")]
  public class SaveRetraitWorker : IAsyncZeebeWorkerWithResult<JsonNode>
  {
    private readonly MockService mockService;
    public SaveRetraitWorker(MockService mockService)
    {
      this.mockService = mockService;
    }
    public async Task<JsonNode> HandleJob(ZeebeJob job, CancellationToken cancellationToken)
    {
      JsonNode variables = job.getVariables<JsonNode>();
      variables["statut"] = "validée";
      mockService.Save("demandes", variables["client"].ToString()+new DateTime().ToString(), variables);
      return variables;
    }
  }
}

