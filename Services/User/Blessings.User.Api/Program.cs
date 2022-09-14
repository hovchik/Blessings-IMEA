using Blessings.Contract.Settings;
using MediatR;
using System.Reflection;

namespace Blessings.User.Api
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
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
            IConfiguration appSettingConfiguration = builder.Configuration.GetSection("AppSetting");
            builder.Services.Configure<AppSetting>(appSettingConfiguration);
            var appSetting = appSettingConfiguration.Get<AppSetting>();
            builder.Services.AddJwtAuthentication(appSetting.JwtTokenConfig);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}