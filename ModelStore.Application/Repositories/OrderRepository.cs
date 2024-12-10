using Dapper;
using ModelStore.Application.Database;
using ModelStore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public OrderRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            var orders = await connection.QueryAsync<Order>(
                new CommandDefinition
                ("""
                    SELECT 
                    order_id AS OrderId, 
                    user_id AS UserId, 
                    order_date AS OrderDate, 
                    status AS Status
                    FROM [order]
                """, cancellationToken: token)
            );

            foreach (var order in orders)
            {
                var orderElements = await connection.QueryAsync<OrderElement>(
                    new CommandDefinition
                    ("""
                        SELECT 
                            order_element_id AS OrderElementId, 
                            order_id AS OrderId, 
                            product_id AS ProductId, 
                            quantity
                        FROM order_element 
                        WHERE order_id = @OrderId
                    """, new { order.OrderId },
                    cancellationToken: token)
                );
            
                order.OrderElements.Clear();
                order.OrderElements.AddRange(orderElements ?? Enumerable.Empty<OrderElement>());
            }

            return orders;
        }
    }
}