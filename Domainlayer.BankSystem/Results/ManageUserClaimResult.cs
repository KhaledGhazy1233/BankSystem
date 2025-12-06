using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Results
{
    public class ManageUserClaimResult
    {
        public int UserId { get; set; }

        public List<UserClaim> ? UserClaims { get; set; }
      
    }
    public class UserClaim
    {
        public string ClaimType { get; set; }
        public bool ClaimValue { get; set; }
    }
}
