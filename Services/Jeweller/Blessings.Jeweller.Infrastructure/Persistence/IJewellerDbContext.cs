using Blessings.Jeweller.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blessings.Jeweller.Infrastructure.Persistence;

public interface IJewellerDbContext
{
    DbSet<Domain.Jeweller> Jewellers { get; set; }
    DbSet<OrderProcessing> OrderProcessing { get; set; }
    DbSet<OrderSchedule> OrderSchedules { get; set; }
    DbSet<Material> Materials { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    int SaveChanges();
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

}