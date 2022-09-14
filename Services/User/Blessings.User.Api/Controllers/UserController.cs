using Blessings.User.Api.CQRS.Autentication;
using Blessings.User.Api.CQRS.Managment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blessings.User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginAsync(LoginCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserCommand request)
        {
            if (await _mediator.Send(request))
            {
                return Ok();
            }

            return BadRequest(new Exception("User creation failed"));
        }
    }
}
