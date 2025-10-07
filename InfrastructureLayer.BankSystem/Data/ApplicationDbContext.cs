using Domainlayer.BankSystem.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,Role, int>
    {

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        //public DbSet<Customer> customers { get; set; }

        public DbSet<Transaction> transactions { get; set; }        

        public DbSet<TransactionType> transactionTypes { get; set; }    

        public DbSet<User> users { get; set; }


    }
}
