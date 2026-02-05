namespace BusinessCore.BankSystem.Features.Transaction.Commands.Responses
{
    public class TransferResponse
    {
        public string ReferenceNumber { get; set; } = string.Empty;

        // نوع العملية (إيداع، سحب، تحويل)
        public string TransactionType { get; set; } = string.Empty;

        // المبلغ اللي تم في العملية
        public decimal Amount { get; set; }

        // رقم الحساب "المرسل" (بيكون "Cash" في حالة الإيداع)
        public string FromAccountNumber { get; set; } = "N/A";

        // رقم الحساب "المستلم" (بيكون "Cash" في حالة السحب)
        public string ToAccountNumber { get; set; } = "N/A";

        // رصيدك "بعد" العملية (اللقطة الحالية)
        public decimal BalanceAfter { get; set; }

        // تاريخ ووقت العملية
        public DateTime TransactionDate { get; set; }

        // حالة العملية (Success / Failed)
        public string Status { get; set; } = "Success";

        // وصف إضافي (مثلاً: "إيداع نقدي من ATM")
        public string? Description { get; set; }
    }
}
