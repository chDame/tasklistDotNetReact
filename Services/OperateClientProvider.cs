
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace tasklistDotNetReact.Services
{
  public class OperateClientProvider
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
    private const string AuthToken_CacheKey = "Operate_AuthToken";
    private String saasTokenUrl = "https://login.cloud.camunda.io/oauth/token";

    public OperateClientProvider(IMemoryCache memoryCache, ZeebeConfigProvider zeebeConfigProvider)
    {
      _memoryCache = memoryCache;
      this.zeebeConfigProvider = zeebeConfigProvider;
    }

    public async Task<HttpClient> GetClientAsync()
    {
      var bearerValue = await GetToken();
      var token = $"Bearer {bearerValue}";
      HttpClient httpClient = new HttpClient();
      httpClient.BaseAddress = new Uri(zeebeConfigProvider.GetOperateUrl());
      httpClient.DefaultRequestHeaders.Accept.Clear();
      httpClient.DefaultRequestHeaders.Add("Authorization", token);

      return httpClient;
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

      var collection = new credentials { client_id = cloudClientid, client_secret = cloudClientSecret, grant_type = "client_credentials", audience = "operate.camunda.io" };

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

