using OnlineBankingApplication.Infrastructure.Models.Accounts.Requests;
using OnlineBankingApplication.Infrastructure.Models.Accounts.Responses;

namespace OnlineBankingApplication.Infrastructure.Services.AccountServices;

public interface IAccountService
{
    Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request);
    Task<DepositMoneyResponse> DepositMoney(DepositMoneyRequest request);
    Task<WithdrawMoneyResponse> WithdrawMoney(WithdrawMoneyRequest request);
    Task<GetAccountBalanceResponse> GetAccountBalance(GetAccountBalanceRequest request);
}
