using System.ComponentModel.DataAnnotations.Schema;

namespace Domainlayer.BankSystem.Entites
{
    public class Transaction
    {
        public int Id { get; set; }
        public int ToAccountId { get; set; }
        [ForeignKey("ToAccountId")]
        [InverseProperty("SendTransaction")]
        public BankAccount? ToAccount { get; set; }
        public int FromAccountId { get; set; }

        [ForeignKey("FromAccountId")]
        [InverseProperty("ReceiveTransaction")]
        public BankAccount? FromAccount { get; set; }
        public string Status { get; set; } = "Pending"; // الحالة (Pending, Completed, Failed)
        public DateTime? UpdatedAt { get; set; }       // تاريخ آخر تحديث للحالة
        public bool ISDeleted { get; set; } = false;
    }

}
