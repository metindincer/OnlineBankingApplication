namespace OnlineBankingApplication.Infrastructure.Models.Accounts.Responses;

public class WithdrawMoneyResponse
{
    public decimal CurrentBalance { get; set; }
    public string Message { get; set; }
}
