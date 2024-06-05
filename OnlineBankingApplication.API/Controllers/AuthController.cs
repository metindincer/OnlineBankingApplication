using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Infrastructure.Models.Accounts.Requests;
using OnlineBankingApplication.Infrastructure.Models.Users.Requests;
using OnlineBankingApplication.Infrastructure.Services.UserService;

namespace OnlineBankingApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserRequest request)
        {
            await _userService.RegisterUser(request);

            return Ok();
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginUserAsync(LoginUserRequest request)
        {
            return Ok(await _userService.LoginUser(request));
        }
    }
}
