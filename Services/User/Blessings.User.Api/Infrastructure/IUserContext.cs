using Blessings.User.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blessings.User.Api.Infrastructure;

public interface IUserContext
{
    DbSet<Domain.User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}