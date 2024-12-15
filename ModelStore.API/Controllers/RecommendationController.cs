using Microsoft.AspNetCore.Mvc;
using ModelStore.API.Mapping;
using ModelStore.Application.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ModelStore.API.Controllers
{
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public RecommendationController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        [HttpGet(ApiEndpoints.Recommendations.TopSelling)]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] int count = 3, CancellationToken token = default)
        {
            if (count <= 0) count = 3;

            var topProducts = await _orderService.GetTopSellingProductsAsync(count, token);
            if (!topProducts.Any())
                return NotFound("No products found.");

            var productsResponse = topProducts.MapToResponse();
            return Ok(productsResponse);
        }

        [HttpGet(ApiEndpoints.Recommendations.FrequentlyBoughtTogether)]
        public async Task<IActionResult> GetFrequentlyBoughtTogether(Guid productId, [FromQuery] int count = 3, CancellationToken token = default)
        {
            if (count <= 0) count = 3;

            var recommendations = await _orderService.GetFrequentlyBoughtTogetherAsync(productId, count, token);
            if (!recommendations.Any())
                return NotFound($"No products frequently bought with product ID {productId}.");

            var recommendationsResponse = recommendations.MapToResponse();
            return Ok(recommendationsResponse);
        }
    }
}