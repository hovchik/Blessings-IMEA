using Blessings.Contract;
using Blessings.Orders.Infrastructure.Persistence;
using Blessings.Shared;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Blessings.OrdersApi.Consumers;

public class JewellerConsumer : IConsumer<JewellerOrderContract>
{
    private readonly OrderDbContext _context;

    public JewellerConsumer(OrderDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<JewellerOrderContract> context)
    {
        //will add retry police when the execution is failed

        var jsonMessage = JsonConvert.SerializeObject(context.Message);

        var jewellerOrder = JsonConvert.DeserializeObject<JewellerOrderContract>(jsonMessage);

        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == jewellerOrder.OrderId);

        if (order != null && order.Status != OrderStatus.Done)
        {
            order.Status = jewellerOrder.Status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}