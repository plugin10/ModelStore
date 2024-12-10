using ModelStore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);
    }
}