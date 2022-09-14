using Blessings.Jeweller.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blessings.Jeweller.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddJewellerContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<JewellerDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DatabaseSettings")));
        services.AddScoped<IJewellerDbContext, JewellerDbContext>();

        return services;
    }
}