using Domainlayer.BankSystem.Entites;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace Domainlayer.BankSystem.Helper
{
    public class AuditEntry
    {
        public EntityEntry Entry { get; }
        public string TableName { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateTime { get; set; }
        public Dictionary<string, object> OldValues { get; } = new();
        public Dictionary<string, object> NewValues { get; } = new();

        // القائمة دي مهمة جداً للتعامل مع الـ Primary Keys اللي بتتولد أوتوماتيك
        public List<PropertyEntry> TemporaryProperties { get; } = new();

        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public AuditLog ToAudit()
        {
            return new AuditLog
            {
                TableName = TableName,
                Action = Action,
                UserId = UserId,
                IpAddress = IpAddress,
                DateTime = DateTime,
                // تحويل الـ Dictionary لنص JSON
                OldValue = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
                NewValue = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
                ISDeleted = false,
                TransTy = 0
            };
        }
    }
}
