namespace OnlineBankingApplication.Domain.Entities;

public class Users : BaseEntity
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public List<Accounts>? Accounts { get; set; }
}
