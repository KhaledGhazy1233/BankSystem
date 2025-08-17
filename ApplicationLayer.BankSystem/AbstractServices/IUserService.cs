using ApplicationLayer.BankSystem.ServiceBases;
using Domainlayer.BankSystem.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface IUserService : IService<User>
    {

        UserManager<User> UserManager { get; }  
    }
}
