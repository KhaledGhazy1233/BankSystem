using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Results
{
    public class ManageUserRoleResult
    {
        public int UserId { get; set; }
        public List<UserRole> ?userRoles { get; set; }


        public class UserRole
        {
            public int Id { get; set; }
            public string ? RoleName { get; set; }
            public bool HasRole { get; set; }

        }
    }
}
