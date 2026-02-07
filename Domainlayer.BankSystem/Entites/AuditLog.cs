using System.ComponentModel.DataAnnotations.Schema;

namespace Domainlayer.BankSystem.Entites
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string IpAddress { get; set; }
        public int TransTy { get; set; }
        public bool ISDeleted { get; set; } = false;
        public DateTime DateTime { get; set; }
        //public User user { get; set; }
        //public int UserIdLLogs { get; set; }
        [InverseProperty(nameof(ApplicationUser.AuditLogs))]
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}
