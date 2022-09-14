using MediatR;

namespace Blessings.User.Api.CQRS.Autentication;

public class LoginCommand : IRequest<LoginResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}