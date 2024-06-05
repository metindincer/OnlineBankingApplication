using OnlineBankingApplication.Infrastructure.Models.Users.Requests;
using OnlineBankingApplication.Infrastructure.Models.Users.Responses;

namespace OnlineBankingApplication.Infrastructure.Services.UserService;

public interface IUserService
{
    Task RegisterUser(RegisterUserRequest request);
    Task<LoginUserResponse> LoginUser(LoginUserRequest request);
}
