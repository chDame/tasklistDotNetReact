
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace tasklistDotNetReact.Services
{
  public class MockService
  {

    string path = "workspace/mocks/";

    internal string Resolve(string type, string name) => path + type + "/" + name;

    public JsonNode FindByName(string type, string name)
    {
      string content = File.ReadAllText(Resolve(type, name));
      return System.Text.Json.JsonSerializer.Deserialize<JsonNode>(content);
    }

    public List<string> FindNames(string type)
    {
      bool exists = Directory.Exists(path+type);

      if (!exists)
        Directory.CreateDirectory(path+type);
      return Directory
                .EnumerateFiles(path+type, "*", SearchOption.AllDirectories)
                .Select(Path.GetFileName).ToList();
    }

    public void Delete(string type, string name)
    {
      File.Delete(Resolve(type, name));
    }

    public JsonNode Save(string type, string name, JsonNode template) { 

      File.WriteAllText(Resolve(type, name), System.Text.Json.JsonSerializer.Serialize(template));
      return template;
    }
  }
}

