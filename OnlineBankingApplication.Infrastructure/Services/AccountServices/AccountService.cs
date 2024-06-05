using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineBankingApplication.Domain.Entities;
using OnlineBankingApplication.Infrastructure.Models.Accounts.Requests;
using OnlineBankingApplication.Infrastructure.Models.Accounts.Responses;
using OnlineBankingApplication.Infrastructure.Persistence;
using Polly;
using Polly.CircuitBreaker;

namespace OnlineBankingApplication.Infrastructure.Services.AccountServices;

public class AccountService : IAccountService
{
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly SqliteConnection _connection;
    public AccountService(IServiceScopeFactory scopeFactory)
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        InitializeDatabase();

        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30)
            );
        _scopeFactory = scopeFactory; //todo
    }

    public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
    {
        var account = new Accounts
        {
            AccountBalance = request.AccountBalance,
            AccountHolderName = request.AccountHolderName,
            CreateDate = DateTime.Now
        };

        return await _circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            using (var context = new OnlineBankingApplicationDbContext(_connection))
            {
                account.AccountNumber = GetUniqueAccountNumber(context);
                await context.Accounts.AddAsync(account);
                await context.SaveChangesAsync();
            }

            return new CreateAccountResponse
            {
                AccountId = account.Id
            };
        });
    }

    public async Task<DepositMoneyResponse> DepositMoney(DepositMoneyRequest request)
    {
        var balance = 0m;
        return await _circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            using (var context = new OnlineBankingApplicationDbContext(_connection))
            {
                var account = context.Accounts.FirstOrDefault(x => x.Id == request.AccountId);

                if (account is null)
                {
                    throw new Exception("Account not found");
                }

                account.AccountBalance += request.Amount;
                account.UpdateDate = DateTime.Now;
                balance = account.AccountBalance;
                context.Accounts.Update(account);

                await context.SaveChangesAsync();
            }

            return new DepositMoneyResponse
            {
                CurrentBalance = balance,
                Message = "Deposit process is success."
            };
        });
    }

    public async Task<GetAccountBalanceResponse> GetAccountBalance(GetAccountBalanceRequest request)
    {
        var balance = 0m;
        return await _circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            using (var context = new OnlineBankingApplicationDbContext(_connection))
            {
                var account = context.Accounts.FirstOrDefault(x => x.Id == request.AccountId);

                if (account is null)
                {
                    throw new Exception("Account not found");
                }
                balance = account.AccountBalance;
            }

            return new GetAccountBalanceResponse
            {
                AccountBalance = balance
            };
        });
    }

    public async Task<WithdrawMoneyResponse> WithdrawMoney(WithdrawMoneyRequest request)
    {
        var balance = 0m;
        return await _circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            using (var context = new OnlineBankingApplicationDbContext(_connection))
            {
                var account = context.Accounts.FirstOrDefault(x => x.Id == request.AccountId);

                if (account is null)
                {
                    throw new Exception("Account not found");
                }

                if (account.AccountBalance < request.Amount)
                {
                    throw new Exception("Insufficient Balance!");
                }
                account.AccountBalance -= request.Amount;
                account.UpdateDate = DateTime.Now;
                balance = account.AccountBalance;
                context.Accounts.Update(account);

                await context.SaveChangesAsync();
            }

            return new WithdrawMoneyResponse
            {
                CurrentBalance = balance,
                Message = "Withdraw process is success."
            };
        });
    }

    private int GetUniqueAccountNumber(OnlineBankingApplicationDbContext context)
    {
        Random random = new();
        int accountNumber;

        do
        {
            accountNumber = random.Next(100000, 1000000);
        }
        while (context.Accounts.Any(x => x.AccountNumber == accountNumber));

        return accountNumber;
    }

    private void InitializeDatabase()
    {
        using (var context = new OnlineBankingApplicationDbContext(_connection))
        {
            context.Database.EnsureCreated();
        }
    }
}
