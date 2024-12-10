using Microsoft.AspNetCore.Mvc;
using ModelStore.API.Mapping;
using ModelStore.Application.Services;

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
            var products = await _orderService.GetAllAsync(token);

            var productsResponse = products.MapToResponse();
            return Ok(productsResponse);
        }
    }
}