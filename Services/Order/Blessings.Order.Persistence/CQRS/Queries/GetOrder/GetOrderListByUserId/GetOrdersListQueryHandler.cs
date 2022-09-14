using AutoMapper;
using Blessings.Order.Core.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blessings.Order.Core.CQRS.Queries;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrderDto>>
{

    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetOrdersListQueryHandler> _logger;

    public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<GetOrdersListQueryHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handle order list getter");
        var orderList = await _orderRepository.GetOrdersByUserId(request.UserId);
        var resultOrderList = _mapper.Map<List<OrderDto>>(orderList);

        return resultOrderList;
    }
}