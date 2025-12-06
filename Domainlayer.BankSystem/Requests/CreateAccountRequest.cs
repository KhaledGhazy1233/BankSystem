using Domainlayer.BankSystem.Enums;

namespace Domainlayer.BankSystem.Requests
{
    public class CreateAccountRequest
    {
        public AccountTypeEnum AccountType { get; set; }
        public CurrencyEnum Currency { get; set; }
        public int UserId { get; set; }
    }
}
