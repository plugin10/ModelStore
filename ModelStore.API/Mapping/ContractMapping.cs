using ModelStore.Application.Models;
using ModelStore.Contracts.Requests;
using ModelStore.Contracts.Responses;

namespace ModelStore.API.Mapping
{
    public static class ContractMapping
    {
        public static Good MapToGood(this CreateGoodRequest request)
        {
            return new Good
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Brand = request.Brand,
                Ammount = request.Ammount,
                Description = request.Description,
            };
        }

        public static Good MapToGood(this UpdateGoodRequest request, Guid id)
        {
            return new Good
            {
                Id = id,
                Name = request.Name,
                Brand = request.Brand,
                Ammount = request.Ammount,
                Description = request.Description,
            };
        }

        public static GoodResponse MapToResponse(this Good good) 
        {
            return new GoodResponse 
            {
                Id = good.Id,
                Name = good.Name,
                Brand = good.Brand,
                Slug = good.Slug,
                Ammount = good.Ammount,
                Description = good.Description 
            };
        }

        public static GoodsResponse MapToResponse(this IEnumerable<Good> goods)
        {
            return new GoodsResponse
            {
                Items = goods.Select(MapToResponse),
            };
        }
    }
}
