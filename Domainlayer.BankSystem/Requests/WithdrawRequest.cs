namespace Domainlayer.BankSystem.Requests
{
    public class WithdrawRequest
    {
        public string AccountNumber { get; set; } = string.Empty;

        // المبلغ المراد سحبه
        public decimal Amount { get; set; }

        // الـ PIN السري (ضروري جداً في السحب عشان الأمان)
        public string Pin { get; set; } = string.Empty;

        // وصف اختياري (مثلاً: سحب لمصاريف الدراسة)
        public string? Description { get; set; }
    }
}
