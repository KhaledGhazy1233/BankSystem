namespace Domainlayer.BankSystem.Requests
{
    public class DepositRequest
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}
