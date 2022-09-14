using Blessings.Contract;
using Blessings.Contract.Settings;
using Blessings.EventBus.Messages;
using Blessings.Order.Core;
using Blessings.Orders.Infrastructure;
using Blessings.OrdersApi.Consumers;
using MassTransit;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Blessings.OrdersApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Blessings Order Api v1",
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
            builder.Services.AddRepos(builder.Configuration);
            builder.Services.AddCoreServices();
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<JewellerConsumer>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ReceiveEndpoint(EventBusConstants.OrderQueue, c =>
                    {
                        c.ConfigureConsumer<JewellerConsumer>(ctx);
                    });
                });
            });

            var app = builder.Build();

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


            app.MapControllers();

            app.Run();
        }
    }
}