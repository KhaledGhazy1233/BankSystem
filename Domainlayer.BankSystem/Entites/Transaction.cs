using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Entites
{
    public class Transaction
    {
        public int Id { get; set; }
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public  BankAccount ?ToAccount { get; set; }
        public BankAccount ?FromAccount { get; set; }
        public bool ISDeleted { get; set; } = false;
    }

}
