using Domainlayer.BankSystem.Enums;

namespace BusinessCore.BankSystem.Features.BankAccount.Queries.Responses
{
    public class BankAccountResponse
    {
        public int Id { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public CurrencyEnum Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedData { get; set; }
        public DateTime UpdatedData { get; set; }
        public int UserId { get; set; }
    }
}
