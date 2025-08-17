using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Entites
{
    public class ApplicationUser :IdentityUser<int>
    {
        public string FullName { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
    }
}
