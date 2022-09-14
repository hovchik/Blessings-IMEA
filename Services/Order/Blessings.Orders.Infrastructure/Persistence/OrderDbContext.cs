using Blessings.Domain;
using Blessings.Orders.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace Blessings.Orders.Infrastructure.Persistence;

public class OrderDbContext : DbContext
{
    public DbSet<Domain.Order> Orders { get; set; }
    public DbSet<ProductItem> ProductItems { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Set> Sets { get; set; }

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelBuilder(modelBuilder);
        base.OnModelCreating(modelBuilder);
        new DbInitializer(modelBuilder).Seed();
    }

    private void ModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Order>()
            .HasOne(x => x.Set)
            .WithMany(x => x.Orders);
    }
}