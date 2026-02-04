namespace UserLogsSystem.DTO;

public class UserDTO
{
  public Guid id {get; set; }
  public string firstName {get; set; } = string.Empty;
  public string fullMailingAddress {get; set; } = string.Empty;
}