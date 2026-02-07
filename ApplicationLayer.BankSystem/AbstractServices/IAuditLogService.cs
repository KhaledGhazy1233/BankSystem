using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Requests;
namespace ApplicationLayer.BankSystem.AbstractServices
{
    public interface IAuditLogService
    {
        Task<AuditLog?> GetAuditLogByIdAsync(int id);

        // 2. جلب نشاط مستخدم معين (للجدول الرئيسي)
        Task<(List<AuditLog> Items, int TotalCount)> GetUserActivityAsync(GetUserActivityRequest query);
    }
}
