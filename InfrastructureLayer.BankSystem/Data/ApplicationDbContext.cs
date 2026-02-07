using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InfrastructureLayer.BankSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, int>
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor 1: الأساسي اللي بنستخدمه في الـ Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Constructor 2: عشان الـ Migrations تشتغل معاك من غير مشاكل
        public ApplicationDbContext()
        {
        }

        #region Configration

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
        #endregion
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 1. جهّز بيانات المراقبة في الذاكرة (Memory)
            var auditEntries = OnBeforeSaveChanges();

            // 2. حوّل الـ Entries لـ Entities حقيقية وأضفها للـ Context
            if (auditEntries != null && auditEntries.Any())
            {
                foreach (var entry in auditEntries)
                {
                    AuditLogs.Add(entry.ToAudit());
                }
            }

            // 3. احفظ كل حاجة (العملية البنكية + اللوج) في نداء واحد للداتا بيز
            // ده بيضمن إن لو العملية فشلت، اللوج ميتسيفش.. ولو نجحت، الاتنين يتسيفوا سوا
            return await base.SaveChangesAsync(cancellationToken);
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            // سحب الـ User ID مباشرة من الـ HttpContext
            var userIdClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currentUserId = int.TryParse(userIdClaim, out int id) ? id : 0;

            // سحب الـ IP Address مباشرة
            string ipAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "System";

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.Entity is ApplicationUser || entry.Entity is UserRefreshToken || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = currentUserId,
                    IpAddress = ipAddress,
                    DateTime = DateTime.UtcNow
                };

                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        // ملاحظة: دي للمستقبل لو حبيت تسجل الـ ID اللي لسه مطلعش
                        continue;
                    }

                    string propertyName = property.Metadata.Name;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.Action = "Create";
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.Action = "Delete";
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.Action = "Update";
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            return auditEntries;
        }







        #region Declare records

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        //public DbSet<Customer> customers { get; set; }

        public DbSet<Transaction> transactions { get; set; }

        //public DbSet<TransactionType> transactionTypes { get; set; }

        //public DbSet<User> users { get; set; }
        #endregion

    }
}
