using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Infrastructure.Models.Accounts.Requests;
using OnlineBankingApplication.Infrastructure.Services.AccountServices;

namespace OnlineBankingApplication.API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Creates account.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync(CreateAccountRequest request)
        {
            return Ok(await _accountService.CreateAccount(request));
        }

        /// <summary>
        /// Gets account balance.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/balance")]
        public async Task<IActionResult> GetAccountBalanceAsync(Guid id)
        {
            return Ok(await _accountService.GetAccountBalance(new GetAccountBalanceRequest { AccountId = id }));
        }

        /// <summary>
        /// Deposit from account.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> DepositMoneyAsync(Guid id, decimal amount)
        {
            return Ok(await _accountService.DepositMoney(new DepositMoneyRequest { AccountId = id, Amount = amount }));
        }

        /// <summary>
        /// Withdraw to account.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> WithdrawMoneyAsync(Guid id, decimal amount)
        {
            return Ok(await _accountService.WithdrawMoney(new WithdrawMoneyRequest { AccountId = id, Amount = amount }));
        }
    }
}
