using ModelStore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<int> CreateAsync(Order order, CancellationToken token = default);
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);
        Task<Order?> GetByIdAsync(int orderId, CancellationToken token = default);
        Task<List<Guid>> GetFrequentlyBoughtTogetherIdsAsync(Guid productId, int count, CancellationToken token);
        Task<List<Guid>> GetTopSellingProductIdsAsync(int count, CancellationToken token);
    }
}