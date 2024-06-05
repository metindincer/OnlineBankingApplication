namespace OnlineBankingApplication.Infrastructure.Models.Accounts.Requests;

public class WithdrawMoneyRequest
{
    public decimal Amount { get; set; }
    public Guid AccountId { get; set; }
}
