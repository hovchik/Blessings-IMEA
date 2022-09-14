using Blessings.Shared;

namespace Blessings.Order.Core.CQRS.Queries;

public class OrderDto
{
    public int UserId { get; set; }
    public int SetId { get; set; }
    public string Name { get; set; }
    public OrderStatus Status { get; set; }
}