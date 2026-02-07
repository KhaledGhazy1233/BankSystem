using BusinessCore.BankSystem.Features.AuditLog.Queries.Models;
using BusinessCore.BankSystem.Features.AuditLog.Queries.Responses;
using Domainlayer.BankSystem.AppMetaData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : AppControllerBase
    {
        private readonly IMediator _mediator;

        public AuditLogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Query Endpoints (GET)

        // 1. API نشاط المستخدم (الجدول الرئيسي)
        [HttpGet(Router.AuditLog.GetUserActivity)]
        [ProducesResponseType(typeof(Response<List<UserActivityResponse>>), 200)]
        [ProducesResponseType(typeof(Response<string>), 401)]
        [ProducesResponseType(typeof(Response<string>), 403)]
        public async Task<IActionResult> GetUserActivity([FromQuery] GetUserActivityQuery query)
        {
            var response = await _mediator.Send(query);
            return StatusCode((int)response.StatusCode, response);
        }

        // 2. API تفاصيل حركة واحدة (المقارنة)
        [HttpGet(Router.AuditLog.GetById)]
        [ProducesResponseType(typeof(Response<AuditLogDetailResponse>), 200)]
        [ProducesResponseType(typeof(Response<string>), 401)]
        [ProducesResponseType(typeof(Response<string>), 403)]
        [ProducesResponseType(typeof(Response<string>), 404)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var response = await _mediator.Send(new GetAuditLogByIdQuery(id));
            return StatusCode((int)response.StatusCode, response);
        }

        #endregion
    }
}
