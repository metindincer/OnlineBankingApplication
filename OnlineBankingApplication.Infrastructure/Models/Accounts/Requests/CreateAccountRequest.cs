namespace OnlineBankingApplication.Infrastructure.Models.Accounts.Requests;

public class CreateAccountRequest
{
    public string AccountHolderName { get; set; }
    public decimal AccountBalance { get; set; }
}
