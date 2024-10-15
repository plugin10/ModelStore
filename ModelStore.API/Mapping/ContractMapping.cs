using ModelStore.Application.Models;
using ModelStore.Contracts.Requests;

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
                Ammount = request.Ammount,
                Description = request.Description,
            };
        }
    }
}
