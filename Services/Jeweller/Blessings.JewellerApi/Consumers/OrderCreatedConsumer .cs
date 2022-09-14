using Blessings.Contract;
using Blessings.Jeweller.Domain;
using Blessings.Jeweller.Infrastructure.Persistence;
using Blessings.JewellerApi.Services;
using Blessings.Shared;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Blessings.JewellerApi.Consumers;

internal class OrderCreatedConsumer : IConsumer<OrderContract>
{
    private readonly IScheduleOrder<OrderSchedule> _scheduleOrder;
    private readonly IJewellerDbContext _context;
    private readonly IPublishEndpoint _endpoint;
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(IScheduleOrder<OrderSchedule> scheduleOrder, IJewellerDbContext context, IPublishEndpoint endpoint, ILogger<OrderCreatedConsumer> logger)
    {
        _scheduleOrder = scheduleOrder;
        _context = context;
        _endpoint = endpoint;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderContract> context)
    {
        var jsonMessage = JsonConvert.SerializeObject(context.Message);
        _logger.LogInformation($"Object Json: {jsonMessage}");
        OrderContract order = JsonConvert.DeserializeObject<OrderContract>(jsonMessage);

        if (await CheckOrderExist(order))
        {
            _logger.LogWarning($"Order #{order.Id} not found in the Order Service database");
            return;
        }

        OrderStatus status = OrderStatus.Scheduled;
        await using var transaction = await _context.BeginTransactionAsync(CancellationToken.None);
        try
        {
            var availableJeweller = _context.Jewellers.FirstOrDefault(x => x.IsAvailable);
            if (availableJeweller != null)
            {
                status = await GiveOrderToJeweller(availableJeweller, order);
            }
            else
            {
                await ScheduleOrder(order);
            }

            await _context.SaveChangesAsync(new CancellationToken());

            var jewOrdContract = await PublishOrder(order, status);

            await transaction.CommitAsync(CancellationToken.None);
            _logger.LogDebug($"Notify user service about order N{jewOrdContract.OrderId} process.");
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(CancellationToken.None);
            _logger.LogError($"Exception stack trace: {e.StackTrace}");
        }
    }

    private async Task<JewellerOrderContract> PublishOrder(OrderContract order, OrderStatus status)
    {
        var jewOrdContract = new JewellerOrderContract
        {
            OrderId = order.Id,
            Status = status
        };
        _logger.LogDebug($"Transaction comited");

        await _endpoint.Publish(jewOrdContract, CancellationToken.None);
        return jewOrdContract;
    }

    private async Task ScheduleOrder(OrderContract order)
    {
        _logger.LogDebug($"No available Jeweller found, Order N{order.Id} moved to schedule list.");
        var domainOrder = new OrderSchedule
        {
            OrderId = order.Id,
            CreatedDate = DateTime.UtcNow,
            MaterialId = order.MaterialId
        };
        await _scheduleOrder.Add(domainOrder, new CancellationToken());
    }

    private async Task<OrderStatus> GiveOrderToJeweller(Jeweller.Domain.Jeweller availableJeweller, OrderContract order)
    {
        OrderStatus status;
        _logger.LogDebug($"Founded available Jeweller with id: {availableJeweller.Id}");
        var processOrder = new OrderProcessing
        {
            OrderId = order.Id,
            CreatedDate = DateTime.UtcNow,
            Status = OrderStatus.InProcess,
            JewellerId = availableJeweller.Id,
            MaterialId = order.MaterialId
        };

        availableJeweller.IsAvailable = false;
        _context.Jewellers.Update(availableJeweller);
        await _context.OrderProcessing.AddAsync(processOrder);

        status = OrderStatus.InProcess;
        return status;
    }

    private async Task<bool> CheckOrderExist(OrderContract order)
    {
        var existsInSchedule = await _context.OrderSchedules.AnyAsync(x => x.OrderId == order.Id);
        if (existsInSchedule)
        {
            return existsInSchedule;
        }

        return await _context.OrderProcessing.AnyAsync(x => x.OrderId == order.Id);
    }
}