using ApplicationLayer.BankSystem.AbstractServices;
using Domainlayer.BankSystem.Entites;
using Domainlayer.BankSystem.Requests;
using InfrastructureLayer.BankSystem.AbstractRepositories;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.BankSystem.ImplementServices
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repository;
        private readonly ILogger<AuditLogService> _logger; // 1. إضافة الـ Logger

        public AuditLogService(IAuditLogRepository repository, ILogger<AuditLogService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<AuditLog?> GetAuditLogByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to retrieve audit log details for ID: {Id}", id);

                var log = await _repository.GetAuditLogByIdAsync(id);

                if (log == null)
                    _logger.LogWarning("Audit log with ID: {Id} was not found", id);

                return log;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting audit log details for ID: {Id}", id);
                throw; // بنرمي الـ Exception عشان الـ Handler يعرف يتعامل معاه
            }
        }

        public async Task<(List<AuditLog> Items, int TotalCount)> GetUserActivityAsync(GetUserActivityRequest query)
        {
            try
            {
                _logger.LogInformation("Fetching user activity for UserId: {UserId}, Page: {PageNumber}", query.UserId, query.PageNumber);

                var result = await _repository.GetUserActivityAsync(
                    query.UserId,
                    query.PageNumber,
                    query.PageSize,
                    query.FromDate,
                    query.ToDate);

                _logger.LogInformation("Successfully retrieved {Count} records for UserId: {UserId}", result.TotalCount, query.UserId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching activity for UserId: {UserId}", query.UserId);
                throw;
            }
        }
    }
}
