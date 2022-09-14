using Blessings.User.Api.Authentication.Services;
using Blessings.User.Api.CQRS.Autentication;
using MediatR;
using System.Security.Claims;
using Blessings.Shared.Contracts;
using Blessings.User.Api.CQRS.Managment;

namespace Blessings.User.Api.CQRS;

public class TokenProviderCommandHandler : IRequestHandler<TokenProviderCommand, LoginResponse>
{
    private readonly IJwtAuthManager _jwtAuthManager;
    private readonly IRequestHandler<GetRoleByTypeQuery, int> _getRoleHandler;

    public TokenProviderCommandHandler(IJwtAuthManager jwtAuthManager, IRequestHandler<GetRoleByTypeQuery, int> getRoleHandler)
    {
        _jwtAuthManager = jwtAuthManager ?? throw new ArgumentNullException(nameof(jwtAuthManager));
        _getRoleHandler = getRoleHandler;
    }

    public async Task<LoginResponse> Handle(TokenProviderCommand request, CancellationToken cancellationToken)
    {
        var type = await _getRoleHandler.Handle(new GetRoleByTypeQuery { RoleType = (UserRoleType)request.User.RoleId }, cancellationToken);
        var claims = new[]
        {
            new Claim(ClaimTypes.Email,request.User.Email),
            new Claim(ClaimTypes.Name,request.User.Email),
            new Claim(ClaimTypes.NameIdentifier, request.User.Id.ToString()),
            new Claim(ClaimTypes.Role,type.ToString()),
        };
        var jwtResult = _jwtAuthManager.GenerateTokens(request.User.Email, claims, DateTime.UtcNow);

        var returnUser = new LoginResponse
        {
            AccessToken = jwtResult.AccessToken,
            FullName = request.User.FirstName,
            Id = request.User.Id
        };

        return returnUser;
    }
}