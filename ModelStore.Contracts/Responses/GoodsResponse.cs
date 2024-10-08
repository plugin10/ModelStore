using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Responses
{
    public class GoodsResponse
    {
        public required IEnumerable<GoodResponse> Items { get; init; } = Enumerable.Empty<GoodResponse>();
    }
}
