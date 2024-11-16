using Microsoft.AspNetCore.Mvc;
using ModelStore.API.Mapping;
using ModelStore.Application.Models;
using ModelStore.Application.Repositories;
using ModelStore.Contracts.Requests;

namespace ModelStore.API.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository goodRepository)
        {
            _productRepository = goodRepository;
        }

        [HttpPost(ApiEndpoints.Products.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var product = request.MapToGood();

            await _productRepository.CreateAsync(product);

            return CreatedAtAction(nameof(Get), new { idOrSlug = product.Id }, product);
        }

        [HttpGet(ApiEndpoints.Products.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug)
        {
            var product = Guid.TryParse(idOrSlug, out var id)
                ? await _productRepository.GetByIdAsync(id)
                : await _productRepository.GetBySlugAsync(idOrSlug);

            if (product == null)
            {
                return NotFound();
            }

            var response = product.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Products.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();

            var productsResponse = products.MapToResponse();
            return Ok(productsResponse);
        }

        [HttpPut(ApiEndpoints.Products.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody] UpdateProductRequest request)
        {
            var product = request.MapToGood(id);
            var updated = await _productRepository.UpdateProductAsync(product);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete(ApiEndpoints.Products.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _productRepository.DeleteProductAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}