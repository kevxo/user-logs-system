namespace UserLogsSystem.Models;

public class UserLog
{
  public Guid Id { get; set; }
  public Guid UserId { get; set; }
  public string Description { get; set; } = string.Empty;


  public UserLog(Guid id, Guid userId, string description)
  {
    Id = id;
    UserId = userId;
    Description = description;
  }
}