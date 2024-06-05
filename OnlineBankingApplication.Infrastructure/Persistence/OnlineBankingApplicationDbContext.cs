using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineBankingApplication.Domain.Entities;
using System.Reflection;

namespace OnlineBankingApplication.Infrastructure.Persistence;

public class OnlineBankingApplicationDbContext : DbContext
{
    private readonly SqliteConnection _connection;
    public DbSet<Users> Users { get; set; }
    public DbSet<Accounts> Accounts { get; set; }

    public OnlineBankingApplicationDbContext(SqliteConnection connection)
    {
        _connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseSqlite(_connection);
        //builder.UseSqlite("DataSource=:memory:");
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
