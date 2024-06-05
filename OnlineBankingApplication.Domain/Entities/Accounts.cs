namespace OnlineBankingApplication.Domain.Entities;

public class Accounts : BaseEntity
{
    public int AccountNumber { get; set; }
    public string AccountHolderName { get; set; }
    public decimal AccountBalance { get; set; }
    public Users? Users { get; set; }
    public Guid? UserId { get; set; }
}
