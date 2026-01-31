using ApplicationLayer.BankSystem.ModuleDependences;
using BusinessCore.BankSystem;
using BusinessCore.BankSystem.MiddleWare;
using InfrastructureLayer.BankSystem.Data;
using InfrastructureLayer.BankSystem.ModuleDependences;
//using InfrastructureLayer.BankSystem.Seeder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Globalization;

namespace BankSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));



            #region ModelDependences
            builder.Services.AddServiceRegisteration(builder.Configuration)
                            .AddCoreDependencies()
                            .AddServiceDependences(builder.Configuration)
                            .AddInfrastructureDependencies();
            #endregion

            #region Localization
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
            {
            new CultureInfo("en-US"),
            new CultureInfo("de-DE"),
            new CultureInfo("fr-FR"),
            new CultureInfo("ar-EG")
            };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion

            #region logging setting
            // 1. ????? Serilog ???????
            Log.Logger = new LoggerConfiguration()
                // ????? ?? ?? "?????" ???? ????? ????? ???? ?? ???? ?? appsettings.json (??? ???? ??? SQL Server)
                .ReadFrom.Configuration(builder.Configuration)

                // ????????? ???????? ???? ??? ??????? ?? ?????
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext() // ??? ???? ???? ???? ??? UserId ???? IP

                // ??????? ?? ??? Console
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")

                // ??????? ?? ????? (File)
                .WriteTo.File(
                    path: "logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")

                .CreateLogger();

            // 2. ????? .NET ???????? Serilog ????? ?? ??? Logger ?????????
            builder.Host.UseSerilog();
            #endregion




            var app = builder.Build();
            //using (var scope = app.Services.CreateScope())
            //{
            //    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            //    await RoleSeeder.SeedAsync(roleManager);
            //    await UserSeeder.SeedAsync(userManager);
            //}



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            #region Localization Middleware
            var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            #endregion






            app.UseMiddleware<ErrorHandlerMiddleware>();



            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
