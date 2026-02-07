using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.ImplementRepositories;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

    }
}
