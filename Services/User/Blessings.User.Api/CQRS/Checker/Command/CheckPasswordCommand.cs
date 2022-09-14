using MediatR;

namespace Blessings.User.Api.CQRS.Checker;

public class CheckPasswordCommand : IRequest<Domain.User>
{
    public string Email { get; set; }
    public string Password { get; set; }
}