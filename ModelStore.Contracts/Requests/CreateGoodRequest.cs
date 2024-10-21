using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Requests
{
    public class CreateGoodRequest
    {
        public required string Name { get; init; }

        public required string Brand { get; set; }

        public int Ammount { get; init; } = 0;

        public string Description { get; init; } = string.Empty;
    }
}
