using BusinessCore.BankSystem.Features.AuditLog.Queries.Responses;
using Domainlayer.BankSystem.Requests;
using MediatR;

namespace BusinessCore.BankSystem.Features.AuditLog.Queries.Models
{
    public class GetUserActivityQuery : GetUserActivityRequest
                                      , IRequest<Response<List<UserActivityResponse>>>
    {
    }
}
