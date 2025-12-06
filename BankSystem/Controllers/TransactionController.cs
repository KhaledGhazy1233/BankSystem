using MediatR;
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
    }
}
