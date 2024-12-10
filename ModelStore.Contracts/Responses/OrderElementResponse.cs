using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Responses
{
    public class OrderElementResponse
    {
        public int OrderElementId { get; init; }

        public Guid ProductId { get; init; }

        public int Quantity { get; init; }

        public decimal UnitPrice { get; init; }
    }
}