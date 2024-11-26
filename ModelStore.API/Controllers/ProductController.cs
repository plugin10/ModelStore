using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelStore.API.Mapping;
using ModelStore.Application.Models;
using ModelStore.Application.Repositories;
using ModelStore.Application.Services;
using ModelStore.Contracts.Requests;

namespace ModelStore.API.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Administrator,Employee")]
        [HttpPost(ApiEndpoints.Products.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken token)
        {
            var product = request.MapToGood();

            await _productService.CreateAsync(product, token);

            return CreatedAtAction(nameof(Get), new { idOrSlug = product.Id }, product);
        }

        [HttpGet(ApiEndpoints.Products.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
        {
            var product = Guid.TryParse(idOrSlug, out var id)
                ? await _productService.GetByIdAsync(id, token)
                : await _productService.GetBySlugAsync(idOrSlug, token);

            if (product == null)
            {
                return NotFound();
            }

            var response = product.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Products.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var products = await _productService.GetAllAsync(token);

            var productsResponse = products.MapToResponse();
            return Ok(productsResponse);
        }

        [Authorize(Roles = "Administrator,Employee")]
        [HttpPut(ApiEndpoints.Products.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody] UpdateProductRequest request, CancellationToken token)
        {
            var product = request.MapToGood(id);
            var updatedProduct = await _productService.UpdateProductAsync(product, token);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        [Authorize(Roles = "Administrator,Employee")]
        [HttpDelete(ApiEndpoints.Products.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _productService.DeleteProductAsync(id, token);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}