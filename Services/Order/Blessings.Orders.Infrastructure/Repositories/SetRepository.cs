using Blessings.Order.Core.Persistence;
using Blessings.Orders.Infrastructure.Persistence;

namespace Blessings.Orders.Infrastructure.Repositories;

public class SetRepository : BaseRepository<Domain.Set>, ISetRepository
{
    public SetRepository(OrderDbContext context) : base(context)
    {
    }
}