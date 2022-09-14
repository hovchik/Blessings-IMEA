using Blessings.Order.Core.CQRS.Queries.GetSet;
using Blessings.OrdersApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blessings.Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<SetResponse>>> Get()
        {
            return Ok(await _mediator.Send(new GetSetQuery()));
        }
    }
}
