using ApplicationLayer.BankSystem.AbstractServices;
using ApplicationLayer.BankSystem.ServiceBases;
using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class UserService : Service<User>, IUserService
    {

            public UserManager<User> UserManager { get; }

        public UserService(IRepository<User> repository) : base(repository)
        {
        }

       
    }
}
