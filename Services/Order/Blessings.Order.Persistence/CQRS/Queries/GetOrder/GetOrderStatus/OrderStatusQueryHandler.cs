using AutoMapper;
using Blessings.Order.Core.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blessings.Order.Core.CQRS.Queries;

public class OrderStatusQueryHandler : IRequestHandler<OrderStatusQuery, OrderStatusDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OrderStatusQueryHandler> _logger;

    public OrderStatusQueryHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<OrderStatusQueryHandler> logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger;
    }

    public async Task<OrderStatusDto> Handle(OrderStatusQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handle order status for: {request.OrderId}");
        var orderStatus = await _orderRepository.GetAsync(x => x.Id == request.OrderId);
        if (orderStatus == null)
        {
            throw new Exception("Order Not Found");
        }
        var returnValue = _mapper.Map<OrderStatusDto>(orderStatus);

        return returnValue;
    }
}