using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Contracts.Requests
{
    public class CreateOrderElementRequest
    {
        public Guid ProductId { get; init; }

        public int Quantity { get; init; }
    }
}