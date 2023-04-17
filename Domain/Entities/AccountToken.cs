namespace Domain.Entities;

public class AccountToken
{
    public string Token { get; set; }

    public string AccountId { get; set; }
    public Account Account { get; set; }
}