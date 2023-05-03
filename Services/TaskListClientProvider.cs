
using System.Text;
using GraphQL.Client.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using GraphQL.Client.Serializer.Newtonsoft;

namespace tasklistDotNetReact.Services
{
  public class TaskListClientProvider
  {
    public class credentials
    {
      public string client_id { get; set; }
      public string client_secret { get; set; }
      public string grant_type { get; set; }
      public string audience { get; set; }
    }

    private readonly IMemoryCache _memoryCache;
    private readonly ZeebeConfigProvider zeebeConfigProvider;
    private const string AuthToken_CacheKey = "TaskList_AuthToken";
    private String saasTokenUrl = "https://login.cloud.camunda.io/oauth/token";

    public TaskListClientProvider(IMemoryCache memoryCache, ZeebeConfigProvider zeebeConfigProvider)
    {
      _memoryCache = memoryCache;
      this.zeebeConfigProvider = zeebeConfigProvider;
    }

    public async Task<GraphQLHttpClient> GetTaskListClientAsync()
    {
      //TASKLIST API
      var region = zeebeConfigProvider.GetRegion();
      var tasklistBaseUrl = zeebeConfigProvider.GetTaskListUrl();
      var taskListUrl = @tasklistBaseUrl + "/graphql";
      var graphQLHttpClientOptions = new GraphQLHttpClientOptions
      {
        EndPoint = new Uri(taskListUrl)
      };

      var bearerValue = await GetToken();
      var token = $"Bearer {bearerValue}";

      var httpClient = new HttpClient();
      httpClient.DefaultRequestHeaders.Add("Authorization", token);

      var graphQLClient = new GraphQLHttpClient(graphQLHttpClientOptions, new NewtonsoftJsonSerializer(), httpClient);

      return graphQLClient;
    }

    private async Task<string> GetToken()
    {
      if (!_memoryCache.TryGetValue(AuthToken_CacheKey, out string bearerToken))
      {
        bearerToken = await GetNewToken();
        var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddSeconds(280));
        _memoryCache.Set(AuthToken_CacheKey, bearerToken, cacheEntryOptions);
      }
      return bearerToken;
    }

    private async Task<string> GetNewToken()
    {
      var client = new HttpClient();

      var cloudClientid = zeebeConfigProvider.GetClientId();
      var cloudClientSecret = zeebeConfigProvider.GetClientSecret();

      var collection = new credentials { client_id = cloudClientid, client_secret = cloudClientSecret, grant_type = "client_credentials", audience = "tasklist.camunda.io" };

      var stringPayload = JsonConvert.SerializeObject(collection);
      var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

      var response = await client.PostAsync(saasTokenUrl, httpContent);

      response.EnsureSuccessStatusCode();

      var result = await response.Content.ReadAsStringAsync();
      var jsonString = Newtonsoft.Json.JsonConvert.DeserializeObject<SaaSAuth>(result);
      return jsonString.access_token;
    }

  }
}

