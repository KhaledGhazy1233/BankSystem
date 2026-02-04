using BusinessCore.BankSystem.Features.Transaction.Commands.Models;
using BusinessCore.BankSystem.Features.Transaction.Commands.Responses;
using Domainlayer.BankSystem.AppMetaData;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class TransactionController : AppControllerBase
    {
        private readonly IMediator _mediator;
        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #region Transaction Endpoints (POST)

        [HttpPost(Router.Transaction.Transfer)] // المسار اللي إنت معرفه في الـ Router
        [Authorize] // يفضل تكون للمستخدمين المسجلين فقط
        [ProducesResponseType(typeof(Response<TransferResponse>), 200)]
        [ProducesResponseType(typeof(Response<string>), 400)]
        [ProducesResponseType(typeof(Response<string>), 401)]
        public async Task<IActionResult> Transfer([FromBody] TransferCommand command)
        {
            // بنبعت الـ Command للـ Mediator وهو بدوره بيبعته للـ Handler اللي عملناه سوا
            var response = await _mediator.Send(command);

            // بنرجع الـ StatusCode بناءً على اللي راجع من الـ Response Wrapper
            // لو نجح هيرجع 200 (OK)، ولو فشل هيرجع Code الخطأ المناسب
            return StatusCode((int)response.StatusCode, response);
        }


        [HttpPost(Router.Transaction.Deposit)]
        [ProducesResponseType(typeof(Response<TransferResponse>), 200)]
        public async Task<IActionResult> Deposit([FromBody] DepositCommand request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost(Router.Transaction.Withdraw)]
        [ProducesResponseType(typeof(Response<TransferResponse>), 200)]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawCommand request)
        {
            var response = await _mediator.Send(request);
            return StatusCode((int)response.StatusCode, response);
        }

        #endregion
    }
}
