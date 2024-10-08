using Microsoft.AspNetCore.Mvc;
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
            var good = new Good
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Ammount = request.Ammount,
                Description = request.Description,
            };

            await _goodRepository.CreateAsync(good);

            return Created($"api/goods/{good.Id}", good);
        }
    }
}
