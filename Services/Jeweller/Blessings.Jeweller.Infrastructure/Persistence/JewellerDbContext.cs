using Blessings.Jeweller.Domain;
using Blessings.Jeweller.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blessings.Jeweller.Infrastructure.Persistence;

public class JewellerDbContext : DbContext, IJewellerDbContext
{
    public JewellerDbContext(DbContextOptions<JewellerDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelBuilder(modelBuilder);
        base.OnModelCreating(modelBuilder);
        new DbStartupConfigs(modelBuilder).Seed();
    }
    public DbSet<Domain.Jeweller> Jewellers { get; set; }
    public DbSet<OrderProcessing> OrderProcessing { get; set; }
    public DbSet<OrderSchedule> OrderSchedules { get; set; }
    public DbSet<Material> Materials { get; set; }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return this.Database.BeginTransactionAsync(cancellationToken);
    }

    private void ModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.OrderProcessing>()
            .HasOne(x => x.Jeweller)
            .WithMany(x => x.OrderProcessing);
        modelBuilder.Entity<Domain.OrderProcessing>()
            .HasOne(x => x.Material)
            .WithMany(x => x.OrderProcessings);
    }
}