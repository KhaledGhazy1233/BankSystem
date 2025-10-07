using System;
using ApplicationLayer.BankSystem.AbstractServices;
using ApplicationLayer.BankSystem.ImplementServices;
using ApplicationLayer.BankSystem.ModuleDependences;
using BusinessCore.BankSystem;
using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.Data;
using InfrastructureLayer.BankSystem.ModuleDependences;
using InfrastructureLayer.BankSystem.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

         var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                await RoleSeeder.SeedAsync(roleManager);
                 await UserSeeder.SeedAsync(userManager);
            }



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
