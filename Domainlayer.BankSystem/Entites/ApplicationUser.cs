using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Entites
{
    public class ApplicationUser :IdentityUser<int>
    {
        public string Image { get; set; }
        public string FullName { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }


        [InverseProperty(nameof(UserRefreshToken.user))]
        public virtual ICollection<UserRefreshToken> UserRefreshTokens { get; set; }
    }
}
