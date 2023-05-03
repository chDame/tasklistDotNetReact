using System.Text.Json.Nodes;
using tasklistDotNetReact.Dtos;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;

namespace tasklistDotNetReact.Services
{
  [JobType("mock")]
  public class MockZeebeWorker : IAsyncZeebeWorkerWithResult<JsonNode>

  {
    private readonly MailService mailService;
    public MockZeebeWorker(MailService mailService)
    {
      this.mailService = mailService;
    }
    public async Task<JsonNode> HandleJob(ZeebeJob job, CancellationToken cancellationToken)
    {
      // get variables as declared (SimpleJobPayload)
      JsonNode variables = job.getVariables<JsonNode>();
      variables["firstname"] = "bob";
      variables["generator"] = "worker";
      variables["mailId"] = await mailService.sendMailAsync("christophe.dame@camunda.com", "subject", "NewMail", "EN", variables);
      // execute business service etc.
      // var result = await _myApiService.DoSomethingAsync(variables.CustomerNo, cancellationToken);
      return variables;
    }
  }
}

