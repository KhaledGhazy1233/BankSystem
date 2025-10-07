using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.ModuleDependences
{
    public static class IdentityDependences
    {
        public static IServiceCollection AddServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, Role>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            //JWT Authentication
            //services.Configure<JwtSetting>(configuration.GetSection("jwtSettings"));
            //services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSetting>>().Value);

            //services.AddTransient<IJwtToken, JwtToken>();
            //services.AddTransient<IAuthenticationService, AuthenticationService>();


            //var jwtSettings = new JwtSetting();

            //configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
            //services.AddSingleton(jwtSettings);

            return services;
        }
    }
}
