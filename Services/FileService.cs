
namespace tasklistDotNetReact.Services
{
  public class FileService
  {

    private static List<Dictionary<string, string>> files = new List<Dictionary<string, string>>();

    public async Task<Dictionary<string, string>> storeFileAsync(IFormFile formFile)
    {

      var filePath = Path.GetTempFileName();

      using (var stream = System.IO.File.Create(filePath))
      {
        await formFile.CopyToAsync(stream);
      }
      Dictionary<string, string> result = new Dictionary<string, string> { { "name", formFile.FileName }, { "reference", "" + files.Count }, { "path", filePath }, { "contentType", formFile.ContentType } };
      files.Add(result);
      return result;
    }


    public Dictionary<string, string> getFile(int fileId)
    {
      return files[fileId];
    }
  }
}

