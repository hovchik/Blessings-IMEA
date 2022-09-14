using Blessings.User.Api.Cryptography;
using Blessings.User.Api.Infrastructure;
using MediatR;

namespace Blessings.User.Api.CQRS.Managment;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
{
    private readonly IUserContext _context;
    public CreateUserCommandHandler(IUserContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        //should be mapped via mapper
        var iteration = 10000;
        var passwordResult = PasswordHasher.Generate(request.Password, iteration);
        Domain.User user = new Domain.User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            Iteration = iteration,
            LastName = request.LastName,
            Salting = passwordResult.Salting,
            Password = passwordResult.PasswordHash,
            RoleId = (int)request.Role
        };

        await _context.Users.AddAsync(user, cancellationToken);
        var saved = await _context.SaveChangesAsync(cancellationToken) == 1;

        return saved;
    }
}