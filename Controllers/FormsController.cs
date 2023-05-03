using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using tasklistDotNetReact.Services;

namespace tasklistDotNetReact.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FormsController : ControllerBase
  {

    private readonly ILogger<FormsController> _logger;
    private readonly OperateService operateService;
    private readonly FormService formService;

    public FormsController(ILogger<FormsController> logger, OperateService operateService, FormService formService)
    {
      _logger = logger;
      this.operateService = operateService;
      this.formService = formService;
    }

    [HttpGet("/api/edition/forms/names")]
    public List<string> List() => formService.FindNames();

    [HttpPost("/api/edition/forms")]
    public Form Save(Form form) => formService.Save(form);

    [HttpGet("/api/edition/forms/{name}")]
    public Form GetForm(string name) => formService.FindByName(name);

    [HttpDelete("/api/edition/forms/{name}")]
    public void Delete(string name) => formService.Delete(name);


    [HttpGet("/api/forms/{processDefinitionId}/{formKey}")]
    public async Task<string> FormSchema(string processDefinitionId, string formKey)
    {
      return await FormSchema("test", processDefinitionId, formKey);
    }

    [HttpGet("/api/forms/{processName}/{processDefinitionId}/{formKey}")]
    public async Task<string> FormSchema(string processName, string processDefinitionId, string formKey)
    {
      return await LocalizedFormSchema(processName, processDefinitionId, formKey, "en");
    }

    [HttpGet("{processName}/{processDefinitionId}/{formKey}/{locale}")]
    public async Task<string> LocalizedFormSchema(string processName, string processDefinitionId, string formKey, string locale)
    {
      if (formKey.StartsWith("camunda-forms:bpmn:"))
      {
        string formId = formKey.Substring(formKey.LastIndexOf(":") + 1);
        return await operateService.EmbeddedForm(processDefinitionId, formId);
      }

      Form form = formService.FindByName(formKey);
      form.schema["generator"] = form.generator;

      return System.Text.Json.JsonSerializer.Serialize(form.schema);

    }

    [HttpGet("instanciation/{bpmnProcessId}")]
    public string InstanciationForm(string bpmnProcessId)
    {
      Form form = formService.FindByName(bpmnProcessId);
      form.schema["generator"] = form.generator;

      return System.Text.Json.JsonSerializer.Serialize(form.schema);
    }
  }
}