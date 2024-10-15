using Microsoft.AspNetCore.Mvc;
using ModelStore.API.Mapping;
using ModelStore.Application.Models;
using ModelStore.Application.Repositories;
using ModelStore.Contracts.Requests;

namespace ModelStore.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class GoodController : ControllerBase
    {
        private readonly IGoodRepository _goodRepository;

        public GoodController(IGoodRepository goodRepository)
        {
            _goodRepository = goodRepository;
        }

        [HttpPost("goods")]
        public async Task<IActionResult> Create([FromBody]CreateGoodRequest request)
        {
            var good = request.MapToGood();

            await _goodRepository.CreateAsync(good);

            return Created($"api/goods/{good.Id}", good);
        }
    }
}
