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

            return CreatedAtAction(nameof(Get), new {id = good.Id}, good);
        }

        [HttpGet(ApiEndpoints.Goods.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var good = await _goodRepository.GetByIdAsync(id);
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
    }
}
