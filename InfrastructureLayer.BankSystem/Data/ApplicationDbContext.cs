using Domainlayer.BankSystem.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
        
        
        }


        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Customer> customers { get; set; }

        public DbSet<Transaction> transactions { get; set; }        

        public DbSet<TransactionType> transactionTypes { get; set; }    

        public DbSet<User> users { get; set; }


    }
}
