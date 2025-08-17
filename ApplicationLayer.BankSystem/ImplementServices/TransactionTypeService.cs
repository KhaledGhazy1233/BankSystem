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
    public class TransactionTypeService : Service<TransactionType>, ITransactionTypeService
    {
        public TransactionTypeService(IRepository<TransactionType> repository) : base(repository)
        {
        }
    }
}
