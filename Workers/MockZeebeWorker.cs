using tasklistDotNetReact.Dtos;
using Zeebe.Client.Accelerator.Abstractions;
using Zeebe.Client.Accelerator.Attributes;

namespace tasklistDotNetReact.Services
{
    [JobType("mock")]
    public class MockZeebeWorker : IAsyncZeebeWorker
    {
        public Task HandleJob(ZeebeJob job, CancellationToken cancellationToken)
        {
            // get variables as declared (SimpleJobPayload)
            var variables = job.getVariables<SimpleJobPayload> ();

            // execute business service etc.
           // var result = await _myApiService.DoSomethingAsync(variables.CustomerNo, cancellationToken);
            return Task.CompletedTask;
        }
    }
}

