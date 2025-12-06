using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Helper
{
    public class ClaimsStore
    {
        public static List<Claim> Claims = new()
        {
        new Claim("Create User","false"),
        new Claim("Edit User","false"),
        new Claim("Delete User","false"),

        };
    }
}
