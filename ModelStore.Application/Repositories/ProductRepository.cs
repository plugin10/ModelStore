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
                foreach (var category in product.Categories)
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        INSERT INTO categorie (product_id, name)
                        VALUES (@ProductId, @Name);
                        """, new { ProductId = product.Id, Name = category },
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

            var categories = await connection.QueryAsync<string>
                (
                    new CommandDefinition
                    ("""
                        SELECT * FROM categorie WHERE product_id = @id
                    """, new { id },
                    cancellationToken: token)
                );

            foreach (var category in categories)
            {
                product.Categories.Add(category);
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

            var categories = await connection.QueryAsync<string>
                (
                    new CommandDefinition
                    ("""
                        SELECT * FROM categorie WHERE product_id = @id
                    """, new { id = product.Id },
                    cancellationToken: token)
                );

            foreach (var category in categories)
            {
                product.Categories.Add(category);
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

            var result = await connection.QueryAsync
                (
                    new CommandDefinition
                    ("""
                        SELECT p.*,
                        STRING_AGG(c.name, ',') AS categories,
                        AVG(CAST(r.rating_score AS FLOAT)) AS rating_score
                        FROM product p
                        LEFT JOIN categorie c ON p.id = c.product_id
                        LEFT JOIN rating r ON p.id = r.product_id
                        GROUP BY p.id, p.name, p.brand, p.slug, p.price, p.stock, p.description
                    """, cancellationToken: token)
                );

            return result.Select(p => new Product
            {
                Id = p.id,
                Name = p.name,
                Brand = p.brand,
                Rating = (float?)p.rating_score,
                Price = p.price,
                Stock = p.stock,
                Categories = Enumerable.ToList(p.categories.Split(',')),
                Description = p.description,
            });
        }

        public async Task<bool> UpdateProductAsync(Product product, CancellationToken token = default)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync
                (new CommandDefinition
                    ("""
                        DELETE FROM categorie WHERE product_id = @id
                    """, new { id = product.Id },
                    transaction: transaction,
                    cancellationToken: token)
                );

            foreach (var category in product.Categories)
            {
                await connection.ExecuteAsync(
                    new CommandDefinition("""
                        INSERT INTO categorie (product_id, name)
                        VALUES (@ProductId, @Name);
                        """, new { ProductId = product.Id, Name = category },
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
                        DELETE FROM categorie WHERE product_id = @id
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