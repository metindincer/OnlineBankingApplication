using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineBankingApplication.Domain.Entities;
using OnlineBankingApplication.Infrastructure.Models.Users.Requests;
using OnlineBankingApplication.Infrastructure.Models.Users.Responses;
using OnlineBankingApplication.Infrastructure.Persistence;
using Polly;
using Polly.CircuitBreaker;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace OnlineBankingApplication.Infrastructure.Services.UserService;

public class UserService : IUserService
{
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
    private readonly SqliteConnection _connection;
    private readonly IConfiguration _configuration;
    public UserService(IConfiguration configuration)
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
        _configuration = configuration;
    }
    public async Task<LoginUserResponse> LoginUser(LoginUserRequest request)
    {
        return await _circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            using (var context = new OnlineBankingApplicationDbContext(_connection))
            {
                var user = context.Users.FirstOrDefault(x => x.UserName == request.UserName && x.Password == request.Password);

                if (user is null)
                {
                    throw new Exception("User not found");
                }
            }
            return new LoginUserResponse
            {
                Message = "Login Successful",
                Token = CreateToken(request)
            };
        });
    }

    public async Task RegisterUser(RegisterUserRequest request)
    {
        var user = new Users
        {
            UserName = request.UserName,
            Password = request.Password,
            CreateDate = DateTime.Now
        };

        using (var context = new OnlineBankingApplicationDbContext(_connection))
        {
            if (context.Users.Any(x => x.UserName == request.UserName))
            {
                throw new Exception("Username exist");
            }
            context.Users.Add(user);
            context.SaveChanges();
        }
    }

    public string CreateToken(LoginUserRequest request)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretsecretsecretsecretsecretsecretsecret"));
        var signInCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "onlinebanking.com",
            audience: "onlinebanking.com",
            expires: DateTime.Now.AddMinutes(30),
        claims: new[]
        {
            new Claim(ClaimTypes.Name, request.UserName),
        },
        signingCredentials: signInCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    private void InitializeDatabase()
    {
        using (var context = new OnlineBankingApplicationDbContext(_connection))
        {
            context.Database.EnsureCreated();
        }
    }
}
