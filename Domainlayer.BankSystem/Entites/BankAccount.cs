using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Entites
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountType { get; set; }
        public string AccountNumber { get; set; }
        public float Balance { get; set; }
        public string Currency { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedData { get; set; }
        public DateTime UpdatedData { get; set; }
        public int UserId { get; set; }
    
        [ForeignKey("UserId")]
        public User ?user { get; set; }
        public bool ISDeleted { get; set; } = false;
        [InverseProperty("ToAccount")]
        public virtual ICollection<Transaction> ?SendTransaction { get; set; }
        [InverseProperty("FromAccount")]
        public virtual ICollection<Transaction> ?ReceiveTransaction { get; set; }


    }
}
