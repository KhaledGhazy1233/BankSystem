using Domainlayer.BankSystem.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domainlayer.BankSystem.Entites
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public AccountTypeEnum AccountType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public CurrencyEnum Currency { get; set; }

        // --- حقول الأمان والـ PIN ---
        public string Pin { get; set; } = "1234"; // القيمة الافتراضية للـ Migration
        public int FailedPinAttempts { get; set; } = 0;
        public bool IsPinLocked { get; set; } = false;

        // --- حقول التحكم في العمليات ---
        public bool IsActive { get; set; } = true;
        public bool IsLocked { get; set; } = false; // يستخدم أثناء تنفيذ الـ Transaction
        public DateTime? LockedAt { get; set; }

        public DateTime CreatedData { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedData { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        public bool ISDeleted { get; set; } = false;

        // --- العلاقات العكسية ---

        [InverseProperty("FromAccount")]
        public virtual ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();

        [InverseProperty("ToAccount")]
        public virtual ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
    }
}
