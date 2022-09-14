using Blessings.Jeweller.Domain;
using Blessings.JewellerApi.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Blessings.Jeweller.Core;

public static class ServiceRegistration
{
    public static IServiceCollection AddJewellerCore(this IServiceCollection services)
    {
        services.AddScoped<IScheduleOrder<OrderSchedule>, JobScheduler>();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}