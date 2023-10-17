using Microsoft.EntityFrameworkCore;

namespace ImageSystem.Infrastructure;

public class PostgreContext : DbContext
{
    public PostgreContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgreContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
