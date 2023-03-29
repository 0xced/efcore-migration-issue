using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<IssueDbContext>()
    .UseOracle("Data Source=MyOracleDB;Integrated Security=yes;")
    .ConfigureWarnings(builder => builder.Default(WarningBehavior.Throw))
    .Options;

var context = new IssueDbContext(options);
_ = context.Model; // Throws InvalidOperationException here, only with EF Core 7.0.4 (works fine with 7.0.3)

public record Issue(int Id, string? Name, double? Weight);

public class IssueDbContext : DbContext
{
    public IssueDbContext(DbContextOptions<IssueDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<Issue>();
        builder.ToTable("ISSUE");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("ISSUE_ID");
        builder.Property(i => i.Name).HasColumnName("NAME");
        builder.Property(i => i.Weight).HasColumnName("WEIGHT").HasColumnType("NUMBER(8,5)");
    }
}