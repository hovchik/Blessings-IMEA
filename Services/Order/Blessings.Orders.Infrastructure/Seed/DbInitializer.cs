using Blessings.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blessings.Orders.Infrastructure.Seed;

public class DbInitializer
{
    private readonly ModelBuilder modelBuilder;

    public DbInitializer(ModelBuilder modelBuilder)
    {
        this.modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        modelBuilder.Entity<Material>().HasData(
           new List<Material>
           {
               new()
               {
                   Id = 1,
                   Name = "Gold",
                   Description = "995"
               },
               new()
               {
                   Id=2,
                   Name = "Silver",
                   Description = "850"
               },
               new()
               {
                   Id=3,
                   Name = "Copper",
                   Description = "908"
               },
           }
        );

        modelBuilder.Entity<Set>().HasData(
            new List<Set>
            {
                new()
                {
                    Id = 1,
                    MaterialId = 1,
                    Name = "Gold set for Woman",
                },
                new()
                {
                    Id = 2,
                    MaterialId = 2,
                    Name = "Silver set for Child",
                },
                new()
                {
                    Id = 3,
                    MaterialId = 3,
                    Name = "Copper set for Dogs",
                },
            }
        );
    }
}