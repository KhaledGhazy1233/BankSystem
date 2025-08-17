using System;
using ApplicationLayer.BankSystem.AbstractServices;
using ApplicationLayer.BankSystem.ImplementServices;
using ApplicationLayer.BankSystem.ModuleDependences;
using BusinessCore.BankSystem;
using InfrastructureLayer.BankSystem.Data;
using InfrastructureLayer.BankSystem.ModuleDependences;
using Microsoft.EntityFrameworkCore;

namespace BankSystem
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
           
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext")));



            #region ModelDependences
            builder.Services.AddServiceRegisteration(builder.Configuration)
                            .AddCoreDependencies()
                            .AddServiceDependences(builder.Configuration);
            #endregion

         var app = builder.Build();

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
