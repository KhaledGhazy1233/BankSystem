using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Entites
{
    public class AuditLog
    {
        public int Id{ get; set; }
        public string Action{ get; set; }
        public string TableName{ get; set; }
        public string OldValue { get; set;}
        public string NewValue { get; set;}
        public string IpAddress { get; set;}
        public  int TransTy{ get; set; }
        public int UserId { get; set; }
        public bool ISDeleted { get; set; } = false;
        public DateTime DateTime { get; set; }
        public User user { get; set; }
        public TransactionType ?TransactionType { get; set; }
    }
}
