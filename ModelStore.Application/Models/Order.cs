using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string? ClientName { get; set; }
        public string? ClientEmail { get; set; }
        public string? ClientPhone { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public List<OrderElement> OrderElements { get; set; } = new();
    }

    public enum OrderStatus
    {
        Pending,
        Completed,
        Cancelled
    }
}