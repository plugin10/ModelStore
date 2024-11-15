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
        private readonly IProductRepository _goodRepository;

        public ProductController(IProductRepository goodRepository)
        {
            _goodRepository = goodRepository;
        }

        [HttpPost(ApiEndpoints.Products.Create)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            var good = request.MapToGood();

            await _goodRepository.CreateAsync(good);

            return CreatedAtAction(nameof(Get), new { idOrSlug = good.Id }, good);
        }

        [HttpGet(ApiEndpoints.Products.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug)
        {
            var good = Guid.TryParse(idOrSlug, out var id)
                ? await _goodRepository.GetByIdAsync(id)
                : await _goodRepository.GetBySlugAsync(idOrSlug);

            if (good == null)
            {
                return NotFound();
            }

            var response = good.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Products.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var goods = await _goodRepository.GetAllAsync();

            var goodsResponse = goods.MapToResponse();
            return Ok(goodsResponse);
        }

        [HttpPut(ApiEndpoints.Products.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody] UpdateProductRequest request)
        {
            var good = request.MapToGood(id);
            var updated = await _goodRepository.UpdateProductAsync(good);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete(ApiEndpoints.Products.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _goodRepository.DeleteProductAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}