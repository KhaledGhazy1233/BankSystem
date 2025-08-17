using ApplicationLayer.BankSystem.ServiceBases;
using Domainlayer.BankSystem.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface IBankAccountService : IService<BankAccount>
    {
    }
}
