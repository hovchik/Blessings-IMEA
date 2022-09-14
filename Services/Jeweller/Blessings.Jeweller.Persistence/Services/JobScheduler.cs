using Blessings.Jeweller.Domain;
using Blessings.Jeweller.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Blessings.JewellerApi.Services;

public class JobScheduler : IScheduleOrder<OrderSchedule>
{
    private readonly IJewellerDbContext _context;

    public JobScheduler(IJewellerDbContext context)
    {
        _context = context;
    }


    public async Task Add(OrderSchedule schedule, CancellationToken cancellationToken)
    {
        _context.OrderSchedules.Add(schedule);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<OrderSchedule> GetById(int id, CancellationToken cancellationToken)
    {
        var orderById = await _context.OrderSchedules.FirstOrDefaultAsync(ord => ord.Id == id, cancellationToken: cancellationToken);
        if (orderById == null)
        {
            throw new ArgumentException(nameof(orderById));
        }

        return orderById;
    }

    public async Task<IEnumerable<OrderSchedule>> GetAll(CancellationToken cancellationToken)
    {
        var allOrders = await _context.OrderSchedules.ToListAsync(cancellationToken);
        return allOrders;
    }

    public async Task Update(OrderSchedule schedule, CancellationToken cancellationToken)
    {

        //await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(OrderSchedule schedule, CancellationToken cancellationToken)
    {
        _context.OrderSchedules.Remove(schedule);
        await _context.SaveChangesAsync(cancellationToken);
    }
}