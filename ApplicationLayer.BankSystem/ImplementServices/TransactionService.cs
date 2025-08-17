using ApplicationLayer.BankSystem.AbstractServices;
using ApplicationLayer.BankSystem.ServiceBases;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class TransactionService : Service<Transaction>, ITransactionService
    {
        public TransactionService(IRepository<Transaction> repository) : base(repository)
        {
        }
    }
}
