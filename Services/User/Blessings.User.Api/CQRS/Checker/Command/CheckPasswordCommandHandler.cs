using Blessings.Contract.Settings;
using Blessings.User.Api.Cryptography;
using Blessings.User.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blessings.User.Api.CQRS.Checker;

public class CheckPasswordCommandHandler : IRequestHandler<CheckPasswordCommand, Domain.User>
{
    private readonly IUserContext _context;
    private readonly AppSetting _appSetting;

    public CheckPasswordCommandHandler(IUserContext context, IOptions<AppSetting> options)
    {
        _context = context;
        _appSetting = options.Value;
    }

    public async Task<Domain.User> Handle(CheckPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException(nameof(user));
        }

        if (!PasswordHasher.IsValid(user.Password, request.Password, user.Salting))
        {
            throw new ArgumentException(nameof(user));
        }

        return user;
    }
}