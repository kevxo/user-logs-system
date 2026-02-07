using UserLogsSystem.Models;
using System.Text.Json;
using UserLogsSystem.DTO;

namespace UserLogsSystem.Services;
public interface IUserService
{
  Task<List<User>> GetUsersAsync();
  Task<List<UserDTO>> GetSummaryUsersAsync(int page, int pageSize);
  Task<List<UserLog>> GetUserLogs();
  Task<List<UserLogDTO>> GetUserLogSummary();
  Task<List<string>> GetUserLogsByUser(Guid userId);
}

public class UserService : IUserService
{
  private readonly IWebHostEnvironment _env;

  public UserService(IWebHostEnvironment env)
  {
    _env = env;
  }

  public async Task<List<User>> GetUsersAsync()
  {
    var filePath = Path.Combine(_env.ContentRootPath, "Data/users.json");

    if (!File.Exists(filePath))
    {
      return new List<User>();
    }

    var json = await File.ReadAllTextAsync(filePath);

    return JsonSerializer.Deserialize<List<User>>(
      json,
      new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      }
    ) ?? new List<User>();
  }

  public async Task<List<UserDTO>> GetSummaryUsersAsync(
   int page,
   int pageSize
  )
  {
    var users = await GetUsersAsync();

    var summaryUsers = users.Select(user => new UserDTO
    {
      id = user.Id,
      firstName = user.FirstName,
      fullMailingAddress = $"{user.MailingAddress.Address1}, {user.MailingAddress.Zipcode}"
    })
    .Skip((page -1 ) * pageSize)
    .Take(pageSize);

    return [.. summaryUsers];

  }


  public async Task<List<UserLog>> GetUserLogs()
  {
    var filePath = Path.Combine(_env.ContentRootPath, "Data/userLogs.json");

    if (!File.Exists(filePath))
    {
      return new List<UserLog>();
    }

    var json = await File.ReadAllTextAsync(filePath);

    return JsonSerializer.Deserialize<List<UserLog>>(
      json,
      new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      }
    ) ?? new List<UserLog>();
  }

  public async Task<List<UserLogDTO>> GetUserLogSummary()
  {
    var userLogs = await GetUserLogs();
    var users = await GetUsersAsync();

    var userLogsSummary = userLogs.Join(
      users,
      log => log.UserId,
      user => user.Id,
      (log, user) => new UserLogDTO {UserFirstName = user.FirstName, LogsAmount = userLogs.Count(i => i.UserId == log.UserId).ToString()})
    .DistinctBy(summary => summary.UserFirstName)
    .OrderByDescending(summary => summary.LogsAmount)
    .ThenBy(summary => summary.UserFirstName)
    .Take(3);

    return [.. userLogsSummary];
  }

  public async Task<List<string>> GetUserLogsByUser(Guid userId)
  {
    var userLogs = await GetUserLogs();

    return userLogs.Where(log => log.UserId == userId).Select(log => log.Description).ToList();
  }
}

