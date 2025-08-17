using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Entites
{
    public class Transaction
    {
        public int Id { get; set; }
        public int ToAccountId { get; set; }
        [ForeignKey("ToAccountId")]
        [InverseProperty("SendTransaction")]
        public  BankAccount ?ToAccount { get; set; }
        public int FromAccountId { get; set; }
       
        [ForeignKey("FromAccountId")]
        [InverseProperty("ReceiveTransaction")]
        public BankAccount ?FromAccount { get; set; }
        public bool ISDeleted { get; set; } = false;
    }

}
