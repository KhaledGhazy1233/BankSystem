namespace BusinessCore.BankSystem.Features.AuditLog.Queries.Responses
{
    public class UserActivityResponse
    {
        // 1. المعرف الفريد للحركة (عشان لو حب يضغط Details يبعته للـ API التاني)
        public int Id { get; set; }

        // 2. نوع الحركة (Create, Update, Delete)
        public string Action { get; set; } = string.Empty;

        // 3. اسم الجدول اللي حصل فيه التغيير (مثلاً BankAccount)
        public string TableName { get; set; } = string.Empty;

        // 4. عنوان الـ IP اللي تمت منه الحركة (للأمان)
        public string IpAddress { get; set; } = string.Empty;

        // 5. تاريخ ووقت الحركة بالظبط
        public DateTime DateTime { get; set; }
    }

}
