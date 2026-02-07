using BusinessCore.BankSystem.Features.AuditLog.Queries.Responses;
using MediatR;

namespace BusinessCore.BankSystem.Features.AuditLog.Queries.Models
{
    public class GetAuditLogByIdQuery : IRequest<Response<AuditLogDetailResponse>>
    {
        public int _id { get; set; }

        public GetAuditLogByIdQuery(int id)
        {
            _id = id;
        }
    }
}
