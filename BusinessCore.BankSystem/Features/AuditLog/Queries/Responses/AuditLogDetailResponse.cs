namespace BusinessCore.BankSystem.Features.AuditLog.Queries.Responses
{
    public class AuditLogDetailResponse
    {
        public int Id { get; set; }
        public string Action { get; set; } = string.Empty; // Create, Update, Delete
        public string TableName { get; set; } = string.Empty; // اسم الجدول
        public string UserName { get; set; } = string.Empty; // اسم الموظف اللي عمل الحركة
        public DateTime DateTime { get; set; }
        public string IpAddress { get; set; } = string.Empty;

        // 2. "قلب" الـ Dashboard: التغييرات اللي حصلت
        // الكي (Key) هو اسم الحقل (مثلاً Balance)، والقيمة هي الكائن اللي فيه القديم والجديد
        public Dictionary<string, ValueComparison> Changes { get; set; } = new();
    }
    public class ValueComparison
    {
        public string? OldValue { get; set; } // القيمة قبل التعديل
        public string? NewValue { get; set; } // القيمة بعد التعديل
    }
}
