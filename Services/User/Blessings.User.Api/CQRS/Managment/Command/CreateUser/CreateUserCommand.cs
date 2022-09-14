using Blessings.Shared.Contracts;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Blessings.User.Api.CQRS.Managment;

public class CreateUserCommand : IRequest<bool>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    [Required]
    public UserRoleType Role { get; set; }
}