using System.ComponentModel.DataAnnotations.Schema;

namespace Domainlayer.BankSystem.Entites
{
    public class Transaction
    {
        public int Id { get; set; }

        // مبلغ العملية
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // حالة الرصيد قبل وبعد (مهمة جداً للـ Audit والـ AI)
        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceBefore { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        // الحساب الراسل (Nullable عشان الإيداع)
        public int? FromAccountId { get; set; }
        [ForeignKey("FromAccountId")]
        [InverseProperty("SentTransactions")]
        public virtual BankAccount? FromAccount { get; set; }

        // الحساب المستلم (Nullable عشان السحب)
        public int? ToAccountId { get; set; }
        [ForeignKey("ToAccountId")]
        [InverseProperty("ReceivedTransactions")]
        public virtual BankAccount? ToAccount { get; set; }

        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed
        public string? Description { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool ISDeleted { get; set; } = false;

        // رقم مرجعي للعملية (بيظهر للعميل في الإيصال)
        public string ReferenceNumber { get; set; } = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
    }

}
