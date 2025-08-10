using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domainlayer.BankSystem.Entites
{
    public class TransactionType
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public bool ISDeleted { get; set; } = false;
        public virtual ICollection<AuditLog> ?AuditLogs { get; set; }
    }
}
