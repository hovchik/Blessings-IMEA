using Blessings.User.Api.CQRS.Autentication;
using MediatR;

namespace Blessings.User.Api.CQRS;

public class TokenProviderCommand : IRequest<LoginResponse>
{
    public Domain.User User { get; set; }
}