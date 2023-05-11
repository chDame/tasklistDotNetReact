using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using tasklistDotNetReact.Services;

namespace tasklistDotNetReact.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FileController : ControllerBase
  {

    private readonly ILogger<FileController> _logger;
    private FileService fileService;

    public FileController(ILogger<FileController> logger, FileService fileService)
    {
      _logger = logger;
      this.fileService = fileService;
    }

    [HttpPost("upload"), Consumes("multipart/form-data")]
    public async Task<IActionResult> OnPostUploadAsync()
    {
      foreach (var formFile in Request.Form.Files)
      {
        if (formFile.Length > 0)
        {
          return Ok(await fileService.storeFileAsync(formFile));
        }
      }
      return Ok(null);
    }

      [HttpGet("serve")]
      public IActionResult Download([FromQuery] string fileId)
      {
        Dictionary<string, string> file = fileService.getFile(Int32.Parse(fileId));
        
        var fs = new FileStream(file["path"], FileMode.Open);

        return File(fs, file["contentType"], file["name"]);
      }

  }
}