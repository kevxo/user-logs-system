using Microsoft.AspNetCore.Mvc;
using UserLogsSystem.DTO;
using UserLogsSystem.Services;

namespace UserLogsSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
  private readonly IUserService _userService;

  public UsersController(IUserService userService)
  {
    _userService = userService;
  }

  [HttpGet]
  public async Task<ActionResult<List<UserDTO>>> GetAllUsers(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 3
  )
  {
    var usersList = await _userService.GetSummaryUsersAsync(page, pageSize);

    return Ok(new {users = usersList});
  }

  [HttpGet("/userLogs")]
  public async Task<ActionResult<List<UserLogDTO>>> GetAllUserLogs()
  {
    var userLogs = await _userService.GetUserLogSummary();

    return Ok(new { userLogsSummary = userLogs });
  }
}
