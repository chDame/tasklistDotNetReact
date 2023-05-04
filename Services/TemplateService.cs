
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace tasklistDotNetReact.Services
{
  public class TemplateService
  {

    string path = "workspace/templates/";

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

    public JsonNode Save(string name, JsonNode template) { 

      File.WriteAllText(Resolve(name), System.Text.Json.JsonSerializer.Serialize(template));
      return template;
    }
  }
}

