using Blessings.Shared;

namespace Blessings.Order.Core.CQRS.Queries;

public class OrderStatusDto
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public OrderStatus Status { get; set; }
}