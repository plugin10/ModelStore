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

            return Created($"/{ApiEndpoints.Goods.Create}/{good.Id}", good);
        }
    }
}
