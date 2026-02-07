using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domainlayer.BankSystem.Entites
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Image { get; set; }
        public string FullName { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

        [InverseProperty(nameof(UserRefreshToken.user))]
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = new List<UserRefreshToken>(); // ✅ أضفنا القيمة الابتدائية هنا
    }
}
