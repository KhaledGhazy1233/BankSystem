using ApplicationLayer.BankSystem.ServiceBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface ITransactionService : IService<Transaction>
    {
    }
}
