using Blessings.Contract;
using Blessings.Jeweller.Domain;
using Blessings.Jeweller.Infrastructure.Persistence;
using Blessings.Shared;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Blessings.JewellerApi.Jobs;

public class JewellerJob : IJewellerJob
{
    private readonly IJewellerDbContext _context;
    private readonly IPublishEndpoint _endpoint;

    public JewellerJob(IJewellerDbContext context, IPublishEndpoint endpoint)
    {
        _context = context;
        _endpoint = endpoint;
    }

    public void JewellerJobAssign()
    {
        var availableJewellers = _context.Jewellers.Where(x => x.IsAvailable).ToList();
        if (!availableJewellers.Any())
        {
            return;
        }

        var scheduledJobs = _context.OrderSchedules.OrderBy(x => x.CreatedDate).ToList();
        if (!scheduledJobs.Any())
        {
            return;
        }

        int i = 0;
        do
        {
            CreateOrderProcessing(availableJewellers[i], scheduledJobs[i]);
            i++;
        }
        while (availableJewellers.Count > i && scheduledJobs.Count > i);
    }

    private void CreateOrderProcessing(Jeweller.Domain.Jeweller jeweller, OrderSchedule orderSchedule)
    {
        try
        {
            jeweller.IsAvailable = false;
            _context.Jewellers.Update(jeweller);

            _context.OrderProcessing.Add(new OrderProcessing
            {
                OrderId = orderSchedule.OrderId,
                Status = OrderStatus.InProcess,
                CreatedDate = DateTime.UtcNow,
                JewellerId = jeweller.Id,
                MaterialId = orderSchedule.MaterialId
            });
            _context.OrderSchedules.Remove(orderSchedule);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.StackTrace}");
        }

    }

    public void UpdateJewellerStatus()
    {
        var inProcessOrders = _context
            .OrderProcessing.Include(x => x.Material)
            .Include(y => y.Jeweller)
            .Where(x => x.Status == OrderStatus.InProcess && x.CreatedDate.AddDays(x.Material.EstimatedDay) <= DateTime.UtcNow)
            .ToList();
        try
        {
            if (inProcessOrders.Any())
            {
                foreach (var orderProcessing in inProcessOrders)
                {
                    orderProcessing.Status = OrderStatus.Done;
                    orderProcessing.Jeweller.IsAvailable = true;
                    PublishStatus(orderProcessing);
                }

                _context.OrderProcessing.UpdateRange(inProcessOrders);
                _context.SaveChanges();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.StackTrace}");
        }
    }

    private void PublishStatus(OrderProcessing orderProcessing)
    {
        _endpoint.Publish(new JewellerOrderContract
        {
            OrderId = orderProcessing.OrderId,
            Status = orderProcessing.Status
        }, CancellationToken.None);
    }
}