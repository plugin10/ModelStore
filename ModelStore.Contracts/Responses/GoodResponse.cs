using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Responses
{
    public class GoodResponse
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }

        public required string Brand { get; set; }

        public required string Slug { get; set; }

        public int Ammount { get; init; } = 0;

        public string Description { get; init; } = string.Empty;
    }
}
