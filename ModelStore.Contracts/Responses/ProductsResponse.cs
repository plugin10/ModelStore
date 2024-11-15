using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Responses
{
    public class ProductsResponse
    {
        public IEnumerable<ProductResponse> Items { get; init; } = Enumerable.Empty<ProductResponse>();
    }
}