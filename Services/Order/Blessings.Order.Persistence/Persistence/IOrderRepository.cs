namespace Blessings.Order.Core.Persistence;

public interface IOrderRepository : IAsyncOrderRepository<Domain.Order>
{
    Task<IEnumerable<Domain.Order>> GetOrdersByUserId(int id);
}