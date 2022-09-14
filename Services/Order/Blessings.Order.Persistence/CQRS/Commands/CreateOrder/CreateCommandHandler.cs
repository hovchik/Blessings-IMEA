using AutoMapper;
using Blessings.Contract;
using Blessings.Order.Core.Persistence;
using Blessings.OrdersApi.Models;
using Blessings.Shared;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blessings.Order.Core.CQRS.Commands.CreateOrder;

public class CreateCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISetRepository _setRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCommandHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CreateCommandHandler> logger, IPublishEndpoint publishEndpoint, ISetRepository setRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _publishEndpoint = publishEndpoint;
        _setRepository = setRepository;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handle order creation at: {DateTime.UtcNow}");

        Domain.Order newOrder = new Domain.Order
        {
            Name = request.Name,
            SetId = request.SetId,
            UserId = request.UserId,
            Status = OrderStatus.Created,
            CreatedDate = DateTime.UtcNow
        };
        var createdOrder = await _orderRepository.AddAsync(newOrder);
        _logger.LogInformation($"Order with ID: {createdOrder.Id} created at: {DateTime.UtcNow}");

        if (createdOrder == null)
        {
            throw new InvalidOperationException(nameof(createdOrder));
        }

        var setForOrder = await _setRepository.GetByIdAsync(request.SetId);
        if (setForOrder == null)
        {
            throw new ArgumentException(nameof(setForOrder));
        }
        var res = new OrderContract
        {
            Id = createdOrder.Id,
            MaterialId = setForOrder.MaterialId,
            Name = createdOrder.Name,
            Price = createdOrder.Price,
            Status = OrderStatus.Created,
        };

        await _publishEndpoint.Publish(res, cancellationToken);
        _logger.LogInformation($"Order published at: {DateTime.UtcNow}");

        var returnOrder = _mapper.Map<OrderResponse>(createdOrder);

        return returnOrder;
    }
}