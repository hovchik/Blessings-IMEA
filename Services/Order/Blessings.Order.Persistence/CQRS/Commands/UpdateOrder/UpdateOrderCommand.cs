using Blessings.Shared;
using MediatR;

namespace Blessings.Order.Core.CQRS.Commands.UpdateOrder;

public class UpdateOrderCommand : IRequest
{
    public OrderStatus Status { get; set; }
}