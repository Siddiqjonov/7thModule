using ContactSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
