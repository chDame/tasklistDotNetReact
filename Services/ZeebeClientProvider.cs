using System;
using Zeebe.Client;
using Zeebe.Client.Impl.Builder;

namespace tasklistDotNetReact.Services
{
  public class ZeebeClientProvider
  {
    private readonly ZeebeConfigProvider zeebeConfigProvider;

    public ZeebeClientProvider(ZeebeConfigProvider zeebeConfigProvider)
    {
      this.zeebeConfigProvider = zeebeConfigProvider;
    }

    public IZeebeClient GetZeebeClient()
    {

      var region = zeebeConfigProvider.GetRegion();
      var clusterId = zeebeConfigProvider.GetClusterId();
      var ClusterAddress = $"{clusterId}.{region}.zeebe.camunda.io:443";

      return CamundaCloudClientBuilder.Builder()
                        .UseClientId(zeebeConfigProvider.GetClientId())
                        .UseClientSecret(zeebeConfigProvider.GetClientSecret())
                        .UseContactPoint(ClusterAddress)
                        .Build();

    }
  }
}

