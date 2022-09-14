using Blessings.User.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Blessings.User.Api.CQRS.Managment;

public class GetRoleByTypeQueryHandler : IRequestHandler<GetRoleByTypeQuery, int>
{
    private readonly IUserContext _context;

    public GetRoleByTypeQueryHandler(IUserContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetRoleByTypeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles.FirstOrDefaultAsync(role => role.Type == (int)request.RoleType, cancellationToken: cancellationToken);
        if (entity == null)
        {
            throw new Exception("Entity not found");
        }

        return entity.Type;
    }
}