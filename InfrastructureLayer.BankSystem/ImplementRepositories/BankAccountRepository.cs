using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.Data;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.BankSystem.ImplementRepositories
{
    public class BankAccountRepository : Repository<BankAccount>, IBankAccountRepository
    {
        private DbSet<BankAccount> _BankAccounts { get; set; }
        public BankAccountRepository(ApplicationDbContext context) : base(context)
        {
            _BankAccounts = context.Set<BankAccount>();
        }

        public async Task<BankAccount> GetByAccountNumberAsync(string accountNumber)
        {
            return await _BankAccounts.
                         FirstOrDefaultAsync(x => x.AccountNumber == accountNumber && !x.ISDeleted);
        }

        public async Task<IEnumerable<BankAccount>> GetAccountsByUserIdAsync(int userId)
        {
            return await
                    _BankAccounts.Where(x => x.UserId == userId && !x.ISDeleted)
                    .Include(x => x.User)
                    .ToListAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetActiveAccountsAsync()
        {
            return await
                   _BankAccounts.Where(x => x.IsActive && !x.ISDeleted)
                   .ToListAsync();
        }
        public async Task<BankAccount?> GetUserIdAsync(int id)
        {
            return await _BankAccounts
                .Include(x => x.User) // الربط مع الـ ApplicationUser اللي عملناه
                .FirstOrDefaultAsync(x => x.UserId == id && !x.ISDeleted);
        }
    }
}
