using Blessings.Contract;
using Blessings.Jeweller.Core.CQRS;
using Blessings.Shared.Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blessings.Jeweller.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JewellerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JewellerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize(UserRoleType.SuperAdmin)]
        [HttpPost("AddJeweller")]
        public async Task<ActionResult<CreateJewellerResponse>> Post(CreateJewellerCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
