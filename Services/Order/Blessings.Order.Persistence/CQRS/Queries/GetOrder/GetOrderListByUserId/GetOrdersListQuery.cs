using MediatR;

namespace Blessings.Order.Core.CQRS.Queries;

public class GetOrdersListQuery : IRequest<List<OrderDto>>
{
    public int UserId { get; set; }

    public GetOrdersListQuery(int userId)
    {
        UserId = userId;
    }
}