using Blessings.User.Api.CQRS.Checker;
using MediatR;

namespace Blessings.User.Api.CQRS.Autentication;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IRequestHandler<TokenProviderCommand, LoginResponse> _tokenProviderHandler;
    private readonly IRequestHandler<CheckPasswordCommand, Domain.User> _checkPasswordHandler;

    public LoginCommandHandler(IRequestHandler<TokenProviderCommand, LoginResponse> tokenProviderHandler, IRequestHandler<CheckPasswordCommand, Domain.User> checkPasswordHandler)
    {
        _tokenProviderHandler = tokenProviderHandler ?? throw new ArgumentNullException(nameof(tokenProviderHandler));
        _checkPasswordHandler = checkPasswordHandler ?? throw new ArgumentNullException(nameof(checkPasswordHandler));
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _checkPasswordHandler.Handle(
            new CheckPasswordCommand { Email = request.Email, Password = request.Password }, cancellationToken);

        var loginResponse = await _tokenProviderHandler.Handle(
            new TokenProviderCommand
            { User = user },
            cancellationToken);
        return loginResponse;
    }
}