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
                        status AS Status,
                        client_name AS ClientName,
                        client_email AS ClientEmail,
                        client_phone AS ClientPhone,
                        shipping_address AS ShippingAddress
                    FROM [order]
                """, cancellationToken: token)
            );

            foreach (var order in orders)
            {
                var orderElements = await connection.QueryAsync<OrderElement>(
                    new CommandDefinition
                    ("""
                        SELECT
                            oe.order_element_id AS OrderElementId,
                            oe.order_id AS OrderId,
                            oe.product_id AS ProductId,
                            oe.quantity,
                            p.price AS UnitPrice
                        FROM order_element oe
                        INNER JOIN product p ON oe.product_id = p.id
                        WHERE oe.order_id = @OrderId
                    """, new { order.OrderId },
                    cancellationToken: token)
                );

                order.OrderElements.Clear();
                order.OrderElements.AddRange(orderElements ?? Enumerable.Empty<OrderElement>());
            }

            return orders;
        }

        public async Task<Order?> GetByIdAsync(int orderId, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

            var order = await connection.QuerySingleOrDefaultAsync<Order>(
                new CommandDefinition
                ("""
                    SELECT
                        order_id AS OrderId,
                        user_id AS UserId,
                        order_date AS OrderDate,
                        status AS Status,
                        client_name AS ClientName,
                        client_email AS ClientEmail,
                        client_phone AS ClientPhone,
                        shipping_address AS ShippingAddress
                    FROM [order]
                    WHERE order_id = @OrderId
                """, new { OrderId = orderId }, cancellationToken: token)
                );

            if (order == null)
                return null;

            var orderElements = await connection.QueryAsync<OrderElement>(
                new CommandDefinition
                ("""
                    SELECT
                        oe.order_element_id AS OrderElementId,
                        oe.order_id AS OrderId,
                        oe.product_id AS ProductId,
                        oe.quantity,
                        p.price AS UnitPrice
                    FROM order_element oe
                    INNER JOIN product p ON oe.product_id = p.id
                    WHERE oe.order_id = @OrderId
                """, new { OrderId = orderId }, cancellationToken: token)
                );

            order.OrderElements.Clear();
            order.OrderElements.AddRange(orderElements ?? Enumerable.Empty<OrderElement>());

            return order;
        }

        public async Task<int> CreateAsync(Order order, CancellationToken token = default)
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var orderId = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    """
                        INSERT INTO [order] (user_id, order_date, status, client_name, client_email, client_phone, shipping_address)
                        OUTPUT INSERTED.order_id
                        VALUES (@UserId, @OrderDate, @Status, @ClientName, @ClientEmail, @ClientPhone, @ShippingAddress)
                    """,
                    new
                    {
                        order.UserId,
                        order.OrderDate,
                        Status = order.Status.ToString(),
                        order.ClientName,
                        order.ClientEmail,
                        order.ClientPhone,
                        order.ShippingAddress
                    },
                    transaction: transaction,
                    cancellationToken: token
                )
            );

            foreach (var element in order.OrderElements)
            {
                await connection.ExecuteAsync(
                    new CommandDefinition(
                        """
                            INSERT INTO order_element (order_id, product_id, quantity)
                            VALUES (@OrderId, @ProductId, @Quantity)
                        """,
                        new
                        {
                            OrderId = orderId,
                            element.ProductId,
                            element.Quantity
                        },
                        transaction: transaction,
                        cancellationToken: token
                    )
                );
            }

            transaction.Commit();
            return orderId;
        }
    }
}