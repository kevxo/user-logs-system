namespace UserLogsSystem.Models;

public class Address
{
  public string Address1 {get; set; } = string.Empty;
  public string Zipcode {get; set; } = string.Empty;

}

public class User
{
  public Guid Id { get; set; }
  public string FirstName { get; set; } = string.Empty;
  public Address MailingAddress { get; set; } = new();
  public Address BillingAddress { get; set; } = new();

  public User(Guid id, string firstName, Address mailingAddress, Address billingAddress)
  {
    Id = id;
    FirstName = firstName;
    MailingAddress = mailingAddress;
    BillingAddress = billingAddress;
  }
}