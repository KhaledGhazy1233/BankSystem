using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCore.BankSystem.Mapping.ApplicationUser
{
    public partial class ApplicationUserProfile :Profile
    {
        public ApplicationUserProfile()
        {
            GeyUserPaginationmapping();
        }
    }
}
