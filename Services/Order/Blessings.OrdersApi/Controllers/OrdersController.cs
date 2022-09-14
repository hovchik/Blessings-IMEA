using Blessings.Order.Core.CQRS.Commands.CreateOrder;
using Blessings.Order.Core.CQRS.Queries;
using Blessings.OrdersApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Blessings.OrdersApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MediatR.IMediator _mediator;

        public OrdersController(MediatR.IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpGet("GetOrders")]
        [ProducesResponseType(typeof(IEnumerable<Domain.Order>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Domain.Order>>> GetOrdersByUserId(int userId)
        {
            var query = new GetOrdersListQuery(userId);
            var orders = await _mediator.Send(query);

            return Ok(orders);
        }

        [HttpGet("GetOrderStatus")]
        [ProducesResponseType(typeof(IEnumerable<OrderStatusDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<OrderStatusDto>> GetOrderStatus(int orderId)
        {
            var query = new OrderStatusQuery { OrderId = orderId };
            var status = await _mediator.Send(query);

            return Ok(status);
        }

    }
}
