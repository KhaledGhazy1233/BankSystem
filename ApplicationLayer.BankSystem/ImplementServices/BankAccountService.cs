using ApplicationLayer.BankSystem.AbstractServices;
using ApplicationLayer.BankSystem.ServiceBases;
using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class BankAccountService : Service<BankAccount>, IBankAccountService
    {
        public BankAccountService(IRepository<BankAccount> repository) : base(repository)
        {
        }
    }
}
