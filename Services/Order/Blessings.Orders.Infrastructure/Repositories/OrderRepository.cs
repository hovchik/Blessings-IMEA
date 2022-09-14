using Blessings.Order.Core.Persistence;
using Blessings.Orders.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blessings.Orders.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<Domain.Order>, IOrderRepository
{
    public OrderRepository(OrderDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Domain.Order>> GetOrdersByUserId(int id)
    {
        var orderList = await _context.Orders
            .Where(o => o.UserId == id)
            .ToListAsync();
        return orderList;
    }
}