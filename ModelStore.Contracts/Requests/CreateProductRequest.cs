using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Requests
{
    public class CreateProductRequest
    {
        public required string Name { get; init; }

        public required string Brand { get; set; }

        public required decimal Price { get; set; }

        public int Stock { get; init; } = 0;

        public IEnumerable<string> Categories { get; init; } = Enumerable.Empty<string>();

        public string Description { get; init; } = string.Empty;
    }
}