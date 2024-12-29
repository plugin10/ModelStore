using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; init; }

        public string Brand { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; init; } = 0;

        public IEnumerable<int> Categories { get; init; } = Enumerable.Empty<int>();

        public string Description { get; init; } = string.Empty;

        public int? Promotion { get; set; }
    }
}