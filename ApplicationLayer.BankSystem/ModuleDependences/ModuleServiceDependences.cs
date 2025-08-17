using ApplicationLayer.BankSystem.AbstractServices;
using ApplicationLayer.BankSystem.ImplementServices;
using InfrastructureLayer.BankSystem.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.ModuleDependences
{
    public static class ModuleServiceDependences
    {
        public static IServiceCollection AddServiceDependences(this IServiceCollection services , IConfiguration configuration)
        {
            services.Configure<JwtSetting>(configuration.GetSection("jwtSettings"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSetting>>().Value);
            
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            return services;
        }
    }
}
