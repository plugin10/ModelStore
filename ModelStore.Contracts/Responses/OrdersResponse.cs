using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Responses
{
    public class OrdersResponse
    {
        public IEnumerable<OrderResponse> Items { get; init; } = Enumerable.Empty<OrderResponse>();
    }
}