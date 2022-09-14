using MediatR;

namespace Blessings.Order.Core.CQRS.Queries;

public class OrderStatusQuery : IRequest<OrderStatusDto>
{
    public int OrderId { get; set; }
}