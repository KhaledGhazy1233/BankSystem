using Domainlayer.BankSystem.Entites;
using InfrastructureLayer.BankSystem.InfrastructureBases;

namespace InfrastructureLayer.BankSystem.AbstractRepositories
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {

        Task<AuditLog?> GetAuditLogByIdAsync(int id);

        Task<(List<AuditLog> Items, int TotalCount)> GetUserActivityAsync(
            int userId,
            int pageNumber,
            int pageSize,
            DateTime? fromDate,
            DateTime? toDate);
    }
}
