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

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
    }
}