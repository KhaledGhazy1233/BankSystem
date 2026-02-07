using ApplicationLayer.BankSystem.AbstractServices;
using BusinessCore.BankSystem.Features.AuditLog.Queries.Models;
using BusinessCore.BankSystem.Features.AuditLog.Queries.Responses;
using MediatR;
using System.Text.Json;

namespace BusinessCore.BankSystem.Features.AuditLog.Queries.Handler
{
    public class AuditLogQueryHandler : IRequestHandler<GetAuditLogByIdQuery, Response<AuditLogDetailResponse>>
                                      , IRequestHandler<GetUserActivityQuery, Response<List<UserActivityResponse>>>
    {

        private readonly IAuditLogService _service;

        public AuditLogQueryHandler(IAuditLogService service) => _service = service;

        public async Task<Response<AuditLogDetailResponse>> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
        {
            var log = await _service.GetAuditLogByIdAsync(request._id);

            if (log == null)
                return Response<AuditLogDetailResponse>.Failure("Activity log not found");

            // بناء الـ DTO وفك الـ JSON يدوياً هنا
            var response = new AuditLogDetailResponse
            {
                Id = log.Id,
                Action = log.Action,
                TableName = log.TableName,
                UserName = log.User?.UserName ?? "System",
                DateTime = log.DateTime,
                IpAddress = log.IpAddress,
                // ميثود فك الـ JSON اللي شرحناها
                Changes = ParseAuditChanges(log.OldValue, log.NewValue)
            };

            return Response<AuditLogDetailResponse>.Success(response);
        }

        public async Task<Response<List<UserActivityResponse>>> Handle(GetUserActivityQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _service.GetUserActivityAsync(request);

            // 2. الـ Manual Mapping للجدول الرئيسي (بيانات بسيطة وسريعة)
            var mappedData = items.Select(log => new UserActivityResponse
            {
                Id = log.Id,
                Action = log.Action,
                TableName = log.TableName,
                IpAddress = log.IpAddress,
                DateTime = log.DateTime
            }).ToList();

            // 3. التغليف في الـ DrawingResponse الموحد بتاعك
            return Response<List<UserActivityResponse>>.Success(mappedData);
        }

        // ميثود الـ Deserialize المساعدة
        private Dictionary<string, ValueComparison> ParseAuditChanges(string? oldJson, string? newJson)
        {
            var changes = new Dictionary<string, ValueComparison>();
            try
            {
                var oldDict = !string.IsNullOrEmpty(oldJson) ? JsonSerializer.Deserialize<Dictionary<string, object>>(oldJson) : null;
                var newDict = !string.IsNullOrEmpty(newJson) ? JsonSerializer.Deserialize<Dictionary<string, object>>(newJson) : null;

                var keys = (oldDict?.Keys ?? Enumerable.Empty<string>()).Union(newDict?.Keys ?? Enumerable.Empty<string>());

                foreach (var key in keys)
                {
                    changes[key] = new ValueComparison
                    {
                        OldValue = oldDict?.ContainsKey(key) == true ? oldDict[key]?.ToString() : "N/A",
                        NewValue = newDict?.ContainsKey(key) == true ? newDict[key]?.ToString() : "N/A"
                    };
                }
            }
            catch { /* هنهندل لو النص مش JSON */ }
            return changes;
        }
    }
}
