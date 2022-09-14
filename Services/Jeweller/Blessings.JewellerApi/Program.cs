using Blessings.Contract;
using Blessings.Contract.Settings;
using Blessings.EventBus.Messages;
using Blessings.Jeweller.Core;
using Blessings.Jeweller.Infrastructure;
using Blessings.JewellerApi.Consumers;
using Blessings.JewellerApi.Jobs;
using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Blessings.JewellerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //Hangfire
            builder.Services.AddHangfire(x =>
                x.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DatabaseSettings")));
            builder.Services.AddHangfireServer();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Blessings Jeweller Api v1",
                    Description = "Web Api for Blessings"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "BearerFormat",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });
            });
            builder.Services.AddJewellerContext(builder.Configuration);
            builder.Services.AddScoped<IJewellerJob, JewellerJob>();
            builder.Services.AddJewellerCore();

            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<OrderCreatedConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ReceiveEndpoint(EventBusConstants.JewellerQueue, c =>
                    {
                        c.ConfigureConsumer<OrderCreatedConsumer>(ctx);
                    });
                });
            });


            var app = builder.Build();

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "Blessings",
                Authorization = new[]
                {
                    new HangfireCustomBasicAuthenticationFilter{
                        User = builder.Configuration.GetSection("HangfireSettings:UserName").Value,
                        Pass = builder.Configuration.GetSection("HangfireSettings:Password").Value
                    }
                }
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            IConfiguration appSettingConfiguration = builder.Configuration.GetSection("AppSetting");
            var appSetting = appSettingConfiguration.Get<AppSetting>();
            app.UseMiddleware<JwtMiddleware>(appSetting);

            RecurringJob.AddOrUpdate<IJewellerJob>(x => x.JewellerJobAssign(), Cron.Minutely);
            RecurringJob.AddOrUpdate<IJewellerJob>(x => x.UpdateJewellerStatus(), Cron.Minutely);

            app.MapControllers();

            app.Run();
        }
    }
}