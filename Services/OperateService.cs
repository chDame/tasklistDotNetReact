using System;
using GraphQL;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

using GraphQL.Client.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using static tasklistDotNetReact.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Net.Http.Headers;

namespace tasklistDotNetReact.Services
{
  public class OperateService
  {
    private readonly OperateClientProvider operateClientProvider;

    public OperateService(OperateClientProvider operateClientProvider)
    {
      this.operateClientProvider = operateClientProvider;
    }

    public async Task<List<JsonElement>> ProcessDefinitions()
    {
      var client = await operateClientProvider.GetClientAsync();

      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      var sort = new List<Dictionary<string, string>> { new Dictionary<string, string> { { "field", "name" }, { "order", "ASC" } }, new Dictionary<string, string> { { "field", "version" }, { "order", "DESC" } } };
      var search = new Dictionary<string, object>(){
    {"filter", new Dictionary<string, string>{ } },
    {"size", 1000},
    {"sort", sort} };
      //,{ "field":"version","order":"DESC"}
      HttpResponseMessage response = await client.PostAsJsonAsync(client.BaseAddress + "/v1/process-definitions/search", search);
      var responseData = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
      JsonElement definitions = (JsonElement)responseData.GetValueOrDefault("items");
      var result = new List<JsonElement>();
      result.Add(definitions[0]);
      string lastDefId = definitions[0].GetProperty("bpmnProcessId").GetRawText();
      foreach (JsonElement value in definitions.EnumerateArray())
      {
        if (lastDefId != value.GetProperty("bpmnProcessId").GetRawText())
        {
          result.Add(value);
          lastDefId = value.GetProperty("bpmnProcessId").GetRawText();
        }
      }
      return result;

    }

    public async Task<string> EmbeddedForm(string processDefinitionId, string formId)
    {
      var client = await operateClientProvider.GetClientAsync();

      HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "/v1/process-definitions/" + processDefinitionId + "/xml");
      string xmlStr = await response.Content.ReadAsStringAsync();
      XDocument xml = XDocument.Parse(xmlStr);
      IEnumerable<XElement> forms =
            (from el in xml.Root.Elements().First().Elements().First().Elements() where (string)el.Attribute("id") == formId select el);
      return forms.First().Value;
    }
  }
}