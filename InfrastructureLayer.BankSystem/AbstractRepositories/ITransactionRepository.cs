using InfrastructureLayer.BankSystem.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InfrastructureLayer.BankSystem.AbstractRepositories
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
    }
}
