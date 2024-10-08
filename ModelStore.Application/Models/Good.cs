using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Models
{
    public class Good
    {
        public required Guid Id { get; init; }

        public required string Name { get; set; }

        public int Ammount { get; set; }

        public string Description { get; set; }
    }
}
