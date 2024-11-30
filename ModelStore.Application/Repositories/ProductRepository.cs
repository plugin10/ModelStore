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
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnectionFactory _DBconnectionFactory;

        public ProductRepository(IDbConnectionFactory dBconnectionFactory)
        {
            _DBconnectionFactory = dBconnectionFactory;
        }

        public async Task<bool> CreateAsync(Product product, CancellationToken token = default)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition
                ("""
                    INSERT INTO product (id, name, brand, slug, price, stock, description)
                    VALUES (@Id, @Name, @Brand, @Slug, @Price, @Stock, @Description);
                    """, product,
                    transaction: transaction,
                    cancellationToken: token)
                );

            if (result > 0)
            {
                foreach (var categoryId in product.Categories)
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        INSERT INTO product_category (product_id, category_id)
                        VALUES (@ProductId, @CategoryId);
                        """, new { ProductId = product.Id, CategoryId = categoryId },
                        transaction: transaction,
                        cancellationToken: token));
                }
            }
            transaction.Commit();

            return result > 0;
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync(token);
            var product = await connection.QuerySingleOrDefaultAsync<Product>
                (
                    new CommandDefinition
                    ("""
                        SELECT * FROM product WHERE id = @id
                    """, new { id },
                    cancellationToken: token)
                );

            if (product == null)
            {
                return null;
            }

            var categoryIds = await connection.QueryAsync<int>
                (
                    new CommandDefinition
                    ("""
                        SELECT category_id FROM product_category WHERE product_id = @id
                    """, new { id },
                    cancellationToken: token)
                );

            foreach (var categoryId in categoryIds)
            {
                product.Categories.Add(categoryId);
            }

            var rating = await connection.QuerySingleOrDefaultAsync<float?>
                (
                    new CommandDefinition
                    ("""
                        SELECT AVG(CAST(rating_score AS FLOAT)) FROM rating WHERE product_id = @id
                    """, new { id },
                    cancellationToken: token)
                );

            product.Rating = rating;

            return product;
        }

        public async Task<Product?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync(token);
            var product = await connection.QuerySingleOrDefaultAsync<Product>
                (
                    new CommandDefinition
                    ("""
                        SELECT * FROM product WHERE slug = @slug
                    """, new { slug },
                    cancellationToken: token)
                );

            if (product == null)
            {
                return null;
            }

            var categoryIds = await connection.QueryAsync<int>
                (
                    new CommandDefinition
                    ("""
                        SELECT category_id FROM product_category WHERE product_id = @id
                    """, new { id = product.Id },
                    cancellationToken: token)
                );

            foreach (var categoryId in categoryIds)
            {
                product.Categories.Add(categoryId);
            }

            var rating = await connection.QuerySingleOrDefaultAsync<float?>
                (
                    new CommandDefinition
                    ("""
                        SELECT AVG(CAST(rating_score AS FLOAT)) FROM rating WHERE product_id = @id
                    """, new { id = product.Id },
                    cancellationToken: token)
                );

            product.Rating = rating;

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync(token);

            var products = await connection.QueryAsync<Product>(
                new CommandDefinition("""
                SELECT p.*, AVG(CAST(r.rating_score AS FLOAT)) AS rating
                FROM product p
                LEFT JOIN rating r ON p.id = r.product_id
                GROUP BY p.id, p.name, p.brand, p.slug, p.price, p.stock, p.description
                """, cancellationToken: token)
            );

            foreach (var product in products)
            {
                var categoryIds = await connection.QueryAsync<int>(
                    new CommandDefinition("""
                    SELECT category_id FROM product_category WHERE product_id = @id
                    """, new { id = product.Id },
                        cancellationToken: token)
                );

                // Clear existing list and add retrieved categories
                product.Categories.Clear();
                product.Categories.AddRange(categoryIds ?? Enumerable.Empty<int>());
            }

            return products;
        }

        public async Task<bool> UpdateProductAsync(Product product, CancellationToken token = default)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync
                (new CommandDefinition
                    ("""
                        DELETE FROM product_category WHERE product_id = @id
                    """, new { id = product.Id },
                transaction: transaction,
                cancellationToken: token)
            );

            foreach (var categoryId in product.Categories)
            {
                await connection.ExecuteAsync
                    (new CommandDefinition
                        ("""
                            INSERT INTO product_category (product_id, category_id)
                            VALUES (@ProductId, @CategoryId);
                        """, new { ProductId = product.Id, CategoryId = categoryId },
                    transaction: transaction,
                    cancellationToken: token)
                );
            }

            var result = await connection.ExecuteAsync(
                new CommandDefinition("""
                        UPDATE product SET
                        slug = @Slug,
                        name = @Name,
                        brand = @Brand,
                        price = @Price,
                        stock = @Stock,
                        description = @Description
                        WHERE id = @Id;
                        """, product,
                    transaction: transaction,
                    cancellationToken: token)
                );

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> DeleteProductAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync
                (new CommandDefinition
                    ("""
                        DELETE FROM product_category WHERE product_id = @id
                    """, new { id },
                    transaction: transaction,
                    cancellationToken: token)
                );

            await connection.ExecuteAsync(new CommandDefinition
                    ("""
                        DELETE FROM rating WHERE product_id = @id
                    """, new { id },
                    transaction: transaction,
                    cancellationToken: token)
                );

            var result = await connection.ExecuteAsync(new CommandDefinition
                    ("""
                        DELETE FROM product WHERE id = @id
                    """, new { id },
                    transaction: transaction,
                    cancellationToken: token)
                );

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsProductAsync(Guid id, CancellationToken token = default)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync(token);

            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                SELECT COUNT(1) FROM product WHERE id = @id
                """, new { id },
                cancellationToken: token));
        }
    }
}