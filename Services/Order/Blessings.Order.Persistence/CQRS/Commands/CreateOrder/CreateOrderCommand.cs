using Blessings.OrdersApi.Models;
using MediatR;

namespace Blessings.Order.Core.CQRS.Commands.CreateOrder;

public record CreateOrderCommand : IRequest<OrderResponse>
{
    public int UserId { get; set; }
    public int SetId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
}