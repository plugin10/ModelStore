using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Models
{
    public class Order
    {
        public int OrderId { get; init; }

        public int? UserId { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public List<OrderElement> OrderElements { get; init; } = new();
    }

    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}