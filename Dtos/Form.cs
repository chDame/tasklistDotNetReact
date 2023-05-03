
using System.Text.Json.Nodes;

namespace tasklistDotNetReact
{
  public class Form
  {

    public String name { get; set; }

    public DateTime modified { get; set; }

    public JsonNode schema { get; set; }

    public JsonNode previewData { get; set; }

    public String generator { get; set; }
  }
}