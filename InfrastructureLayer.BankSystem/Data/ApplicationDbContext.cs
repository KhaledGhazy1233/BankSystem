using Domainlayer.BankSystem.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureLayer.BankSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int>
    {

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. مهم جداً لتجهيز جداول الـ Identity والـ الأساسيات
            base.OnModelCreating(modelBuilder);

            // 2. تعديلاتك الخاصة لتحويل الـ Enum لنص
            modelBuilder.Entity<BankAccount>()
                .Property(e => e.AccountType)
                .HasConversion<string>();

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.Currency)
                .HasConversion<string>();
            modelBuilder.Entity<BankAccount>()
                .Property(e => e.Balance)
                .HasColumnType("decimal(18,2)"); // عشان الدقة في الحسابات البنكية

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.Property(e => e.IsLocked)
                      .HasDefaultValue(false); // نحدد قيمة افتراضية

                entity.Property(e => e.LockedAt)
                      .IsRequired(false); // نأكد إنه ينفع يكون Null
            });
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        //public DbSet<Customer> customers { get; set; }

        public DbSet<Transaction> transactions { get; set; }

        public DbSet<TransactionType> transactionTypes { get; set; }

        public DbSet<User> users { get; set; }


    }
}
