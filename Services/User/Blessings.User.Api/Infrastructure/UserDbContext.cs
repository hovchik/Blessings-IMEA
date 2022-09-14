using Blessings.Shared.Contracts;
using Blessings.User.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blessings.User.Api.Infrastructure;

public class UserDbContext : DbContext, IUserContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {

    }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Domain.User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelBuilder(modelBuilder);
        SeedDatabase(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void SeedDatabase(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new List<Role>
            {
                new()
                {
                    Id = 1,
                    Name = "Client",
                    Type =(int)UserRoleType.Client
                },
                new()
                {
                    Id=2,
                    Name = "SuperAdmin",
                    Type = (int)UserRoleType.SuperAdmin
                }
            }
        );
    }

    private void ModelBuilder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.User>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Users);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}