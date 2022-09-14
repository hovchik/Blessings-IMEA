using Blessings.Shared.Contracts;
using MediatR;

namespace Blessings.User.Api.CQRS.Managment;

public class GetRoleByTypeQuery : IRequest<int>
{
    public UserRoleType RoleType { get; set; }
}