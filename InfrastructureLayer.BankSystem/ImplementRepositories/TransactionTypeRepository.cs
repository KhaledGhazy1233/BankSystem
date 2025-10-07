using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using InfrastructureLayer.BankSystem.Data;
using InfrastructureLayer.BankSystem.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.ImplementRepositories
{
    public class TransactionTypeRepository : Repository<TransactionType>,ITransactionTypeRepository
    {
        public TransactionTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
