using Newtonsoft.Json;
using Stubble.Core.Builders;
using System.Text.Json.Nodes;

namespace tasklistDotNetReact.Services
{
  public class MailService
  {

    string path = "workspace/mails/";

    internal string Resolve(string name) => path + name;

    public JsonNode FindByName(string name)
    {
      string content = File.ReadAllText(Resolve(name));
      return System.Text.Json.JsonSerializer.Deserialize<JsonNode>(content);
    }

    public List<string> FindNames()
    {
      bool exists = Directory.Exists(path);

      if (!exists)
        Directory.CreateDirectory(path);
      return Directory
                .EnumerateFiles(path, "*", SearchOption.AllDirectories)
                .Select(Path.GetFileName).ToList();
    }

    public void Delete(string name)
    {
      File.Delete(Resolve(name));
    }

    public JsonNode Save(JsonNode mail)
    {
      mail["modified"] = DateTime.Now;

      File.WriteAllText(Resolve(mail["name"].ToString()), System.Text.Json.JsonSerializer.Serialize(mail));
      return mail;
    }

    public string Preview(JsonNode mail)
    {
      string template = mail["htmlTemplate"].ToString();
      JsonNode previewData = mail["previewData"];
      var dynamicObject = JsonConvert.DeserializeObject<dynamic>(previewData.ToJsonString())!;

      var stubble = new StubbleBuilder().Build();
      return stubble.Render(template, dynamicObject);
    }
  }
}

