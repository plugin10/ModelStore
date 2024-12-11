using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Requests
{
    public class CreateOrderRequest
    {
        public int? UserId { get; init; }

        public string? ClientName { get; init; }

        public string? ClientEmail { get; init; }

        public string? ClientPhone { get; init; }

        public string ShippingAddress { get; init; } = string.Empty;

        public IEnumerable<CreateOrderElementRequest> Elements { get; init; } = Enumerable.Empty<CreateOrderElementRequest>();
    }
}