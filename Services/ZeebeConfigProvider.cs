using System;
using Microsoft.Extensions.Configuration;
using Zeebe.Client;
using Zeebe.Client.Impl.Builder;

namespace tasklistDotNetReact.Services
{
  public class ZeebeConfigProvider
  {
    private readonly IConfiguration _configuration;

    public ZeebeConfigProvider(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string GetRegion()
    {
      return _configuration.GetSection("ZeebeConfiguration").GetSection("Client").GetValue<string>("region");
    }
    public string GetClusterId()
    {
      return _configuration.GetSection("ZeebeConfiguration").GetSection("Client").GetValue<string>("clusterId");
    }
    public string GetClientId()
    {
      return _configuration.GetSection("ZeebeConfiguration").GetSection("Client").GetValue<string>("clientId");
    }
    public string GetClientSecret()
    {
      return _configuration.GetSection("ZeebeConfiguration").GetSection("Client").GetValue<string>("clientSecret");
    }
    public string GetTaskListUrl()
    {
      return "https://" + GetRegion() + ".tasklist.camunda.io/" + GetClusterId();
    }
    public string GetOperateUrl()
    {
      return "https://" + GetRegion() + ".operate.camunda.io/" + GetClusterId();
    }
  }
}

