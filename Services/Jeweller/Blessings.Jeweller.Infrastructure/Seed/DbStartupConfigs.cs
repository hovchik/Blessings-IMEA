using Blessings.Jeweller.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blessings.Jeweller.Infrastructure.Seed;

public class DbStartupConfigs
{
    private readonly ModelBuilder _modelBuilder;

    public DbStartupConfigs(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        _modelBuilder.Entity<Material>().HasData(
            new List<Material>
            {
                 new Material
                 {
                    Id = 1,
                    Name = "Gold",
                    Description = "desc gold",
                    EstimatedDay = 5
                 },
                 new Material
                 {
                     Id = 2,
                     Name = "Silver",
                     Description = "desc silver",
                     EstimatedDay = 2
                 },
                 new Material
                 {
                     Id = 3,
                     Name = "Copper",
                     Description = "desc copper",
                     EstimatedDay = 3
                 }
            }
        );
        _modelBuilder.Entity<Domain.Jeweller>().HasData(
            new List<Domain.Jeweller>
            {
                new Domain.Jeweller
                {
                    Id=1,
                    Name = "FirstJew",
                    IsAvailable = true
                },
                new Domain.Jeweller
                {
                    Id=2,
                    Name = "SecondJew",
                    IsAvailable = true
                },
                new Domain.Jeweller
                {
                    Id=3,
                    Name = "ThirdJew",
                    IsAvailable = true
                },
                new Domain.Jeweller
                {
                    Id=4,
                    Name = "FourthJew",
                    IsAvailable = true
                },
                new Domain.Jeweller
                {
                    Id=5,
                    Name = "FifthJew",
                    IsAvailable = true
                }
            }
        );
    }
}