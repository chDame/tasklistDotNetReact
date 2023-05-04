
namespace tasklistDotNetReact.Services
{
  public class FormService
  {

    string path = "workspace/forms/";

    internal string ResolveForm(string name) => path + name;

    public Form FindByName(string name)
    {
      string content = File.ReadAllText(ResolveForm(name));
      return System.Text.Json.JsonSerializer.Deserialize<Form>(content);
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
      File.Delete(ResolveForm(name));
    }

    public Form Save(Form form)
    {
      form.modified = DateTime.Now;

      File.WriteAllText(ResolveForm(form.name), System.Text.Json.JsonSerializer.Serialize(form));
      return form;
    }
  }
}

