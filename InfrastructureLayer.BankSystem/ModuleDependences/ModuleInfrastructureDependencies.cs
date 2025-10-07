using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.ImplementRepositories;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.ModuleDependences
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();    
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient(typeof(IRepository<>),typeof(Repository<>));
            return services;
        }

    }
}
