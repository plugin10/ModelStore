using Microsoft.AspNetCore.Mvc;
using ModelStore.API.Mapping;
using ModelStore.Application.Models;
using ModelStore.Application.Repositories;
using ModelStore.Contracts.Requests;

namespace ModelStore.API.Controllers
{
    [ApiController]
    public class GoodController : ControllerBase
    {
        private readonly IGoodRepository _goodRepository;

        public GoodController(IGoodRepository goodRepository)
        {
            _goodRepository = goodRepository;
        }

        [HttpPost(ApiEndpoints.Goods.Create)]
        public async Task<IActionResult> Create([FromBody]CreateGoodRequest request)
        {
            var good = request.MapToGood();

            await _goodRepository.CreateAsync(good);

            return CreatedAtAction(nameof(Get), new { idOrSlug = good.Id}, good);
        }

        [HttpGet(ApiEndpoints.Goods.Get)]
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


        [HttpGet(ApiEndpoints.Goods.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var goods = await _goodRepository.GetAllAsync();

            var goodsResponse = goods.MapToResponse();
            return Ok(goodsResponse);
        }

        [HttpPut(ApiEndpoints.Goods.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id,
            [FromBody]UpdateGoodRequest request)
        {
            var good = request.MapToGood(id);
            var updated = await _goodRepository.UpdateGoodAsync(good);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        [HttpDelete(ApiEndpoints.Goods.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _goodRepository.DeleteGoodAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
