namespace OnlineBankingApplication.Infrastructure.Models.Accounts.Requests;

public class DepositMoneyRequest
{
    public decimal Amount { get; set; }
    public Guid AccountId { get; set; }
}
