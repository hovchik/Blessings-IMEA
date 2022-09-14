using Blessings.Contract;
using Blessings.User.Api.Authentication.Services;
using Blessings.User.Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blessings.User.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<UserDbContext>(options =>
        {
            options.LogTo(System.Console.WriteLine);
            options.UseSqlServer(
                connectionString,
                b => b.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName));
        });

        services.AddScoped<IUserContext>(provider => provider.GetService<UserDbContext>());

        return services;
    }
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, JwtTokenConfig jwtTokenConfig)
    {

        //string schemeName = nameof(SessionHashAuthenticationSchemeOptions);
        services.AddSingleton(jwtTokenConfig);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(option =>
            {

                option.RequireHttpsMetadata = true;
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(10)
                };
            });
        services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
        services.AddAuthorization();


        return services;
    }
}