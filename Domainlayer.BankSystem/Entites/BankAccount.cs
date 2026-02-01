using Domainlayer.BankSystem.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domainlayer.BankSystem.Entites
{
    public class BankAccount
    {
        public int Id { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public string AccountNumber { get; set; }

        public decimal Balance { get; set; }
        public CurrencyEnum Currency { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedAt { get; set; }
        public DateTime CreatedData { get; set; }
        public DateTime UpdatedData { get; set; }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public bool ISDeleted { get; set; } = false;
        [InverseProperty("ToAccount")]
        public virtual ICollection<Transaction>? SendTransaction { get; set; }
        [InverseProperty("FromAccount")]
        public virtual ICollection<Transaction>? ReceiveTransaction { get; set; }


    }
}
