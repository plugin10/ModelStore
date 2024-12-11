using Microsoft.AspNetCore.Mvc;
using ModelStore.API.Mapping;
using ModelStore.Application.Models;
using ModelStore.Application.Services;
using ModelStore.Contracts.Requests;

namespace ModelStore.API.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet(ApiEndpoints.Orders.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var orders = await _orderService.GetAllAsync(token);

            var ordersResponse = orders.MapToResponse();
            return Ok(ordersResponse);
        }

        [HttpGet(ApiEndpoints.Orders.Get)]
        public async Task<IActionResult> Get([FromRoute] int orderId, CancellationToken token)
        {
            var orders = await _orderService.GetByIdAsync(orderId, token);

            if (orders == null)
            {
                return NotFound();
            }

            var ordersResponse = orders.MapToResponse();
            return Ok(ordersResponse);
        }

        [HttpPost(ApiEndpoints.Orders.Create)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken token)
        {
            var order = request.MapToOrder();

            await _orderService.CreateAsync(order, token);

            return CreatedAtAction(nameof(Get), new { orderId = order.OrderId }, order);
        }
    }
}