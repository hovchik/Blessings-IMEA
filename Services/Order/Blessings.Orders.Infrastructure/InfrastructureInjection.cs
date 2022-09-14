using Blessings.Order.Core.Persistence;
using Blessings.Orders.Infrastructure.Persistence;
using Blessings.Orders.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blessings.Orders.Infrastructure;

public static class InfrastructureInjection
{
    public static IServiceCollection AddRepos(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));
        services.AddScoped(typeof(IAsyncOrderRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ISetRepository, SetRepository>();

        return services;
    }
}