using AutoMapper;
using BusinessCore.BankSystem.Features.User.Queries.Response;
using Domainlayer.BankSystem.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationUser= Domainlayer.BankSystem.Entites ;

namespace BusinessCore.BankSystem.Mapping.ApplicationUser
{
    public partial class ApplicationUserProfile :Profile
    {
        public void GeyUserPaginationmapping()
        {
            CreateMap<Domainlayer.BankSystem.Entites.ApplicationUser, GetUserPaginatedResponse>();//<source,,destination>
        }
    }
}
