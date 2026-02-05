namespace Domainlayer.BankSystem.Requests
{
    public class TransferRequest
    {
        public string? Description { get; set; }

        public string Pin { get; set; }

        public decimal Amount { get; set; }
        public string ToAccountNumber { get; set; }

        public string FromAccountNumber { get; set; }
    }
}
