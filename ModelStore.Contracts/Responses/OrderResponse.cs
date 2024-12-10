using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Responses
{
    public class OrderResponse
    {
        public int OrderId { get; init; }

        public int? UserId { get; init; }

        public DateTime OrderDate { get; init; }

        public string Status { get; init; } = string.Empty;

        public IEnumerable<OrderElementResponse> Elements { get; init; } = Enumerable.Empty<OrderElementResponse>();
    }
}