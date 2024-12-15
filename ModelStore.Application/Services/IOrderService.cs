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
        Task<int> CreateAsync(Order order, CancellationToken token = default);

        Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);

        Task<Order?> GetByIdAsync(int orderId, CancellationToken token = default);
        Task<List<Product>> GetFrequentlyBoughtTogetherAsync(Guid productId, int count, CancellationToken token);
        Task<List<Product>> GetTopSellingProductsAsync(int topCount, CancellationToken token);
    }
}