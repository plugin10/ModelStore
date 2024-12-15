using FluentValidation;
using ModelStore.Application.Models;
using ModelStore.Application.Repositories;
using ModelStore.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;

        public OrderService(IOrderRepository orderRepository, IProductService productService)
        {
            _orderRepository = orderRepository;
            _productService = productService;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default)
        {
            return await _orderRepository.GetAllAsync(token);
        }

        public async Task<Order?> GetByIdAsync(int orderId, CancellationToken token = default)
        {
            return await _orderRepository.GetByIdAsync(orderId, token);
        }

        public async Task<int> CreateAsync(Order order, CancellationToken token = default)
        {
            return await _orderRepository.CreateAsync(order, token);
        }

        public async Task<List<Product>> GetTopSellingProductsAsync(int topCount, CancellationToken token)
        {
            var topProductIds = await _orderRepository.GetTopSellingProductIdsAsync(topCount, token);
            return await _productService.GetProductsByIdsAsync(topProductIds, token);
        }

        public async Task<List<Product>> GetFrequentlyBoughtTogetherAsync(Guid productId, int count, CancellationToken token)
        {
            var relatedProductIds = await _orderRepository.GetFrequentlyBoughtTogetherIdsAsync(productId, count, token);
            return await _productService.GetProductsByIdsAsync(relatedProductIds, token);
        }
    }
}