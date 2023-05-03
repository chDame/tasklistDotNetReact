using Microsoft.AspNetCore.Mvc;
using tasklistDotNetReact.Services;

namespace tasklistDotNetReact.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TasksController : ControllerBase
  {

    private readonly ILogger<TasksController> _logger;
    private readonly TaskListService _taskListService;

    public TasksController(ILogger<TasksController> logger, TaskListService taskListService)
    {
      _logger = logger;
      _taskListService = taskListService;
    }


    [HttpGet]
    public async Task<JsonResult> GetAllTasks()
    {
      var alltasks = await _taskListService.FetchAllTasks();

      return new JsonResult(alltasks);
    }
    [HttpPost("search")]
    public async Task<JsonResult> Search()
    {
      var alltasks = await _taskListService.FetchAllTasks();

      return new JsonResult(alltasks);
    }

    [HttpGet("{taskId}/claim")]
    public async Task<JsonResult> Claim(string taskId)
    {
      Models.Task task = await _taskListService.ClaimTask(taskId, "demo");

      return new JsonResult(task);
    }
    [HttpGet("{taskId}/unclaim")]
    public async Task<JsonResult> Unclaim(string taskId)
    {
      Models.Task task = await _taskListService.UnClaimTask(taskId);

      return new JsonResult(task);
    }

    [HttpPost("{taskId}")]
    public async Task<JsonResult> complete(string taskId, [FromBody] Dictionary<string, string> variables)
    {
      Models.Task task = await _taskListService.CompleteTask(taskId, variables);

      return new JsonResult(task);
    }

  }
}