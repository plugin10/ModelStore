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

        public async Task<bool> CreateAsync(Product product)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition
                ("""
                    INSERT INTO products (id, name, brand, slug, amount, description)
                    VALUES (@Id, @Name, @Brand, @Slug, @Ammount, @Description);
                    """, product,
                    transaction: transaction)
                );

            if (result > 0)
            {
                foreach (var category in product.Categories)
                {
                    await connection.ExecuteAsync(new CommandDefinition("""
                        INSERT INTO categories (productId, name)
                        VALUES (@ProductId, @Name);
                        """, new { ProductId = product.Id, Name = category },
                        transaction: transaction));
                }
            }
            transaction.Commit();

            return result > 0;
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync();
            var product = await connection.QuerySingleOrDefaultAsync<Product>
                (
                    new CommandDefinition
                    ("""
                        SELECT * FROM products WHERE id = @id
                    """, new { id })
                );

            if (product == null)
            {
                return null;
            }

            var categories = await connection.QueryAsync<string>
                (
                    new CommandDefinition
                    ("""
                        SELECT * FROM categories WHERE productId = @id
                    """, new { id })
                );

            foreach (var category in categories)
            {
                product.Categories.Add (category);
            }

            return product;
        }

        public async Task<Product?> GetBySlugAsync(string slug)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync();
            var product = await connection.QuerySingleOrDefaultAsync<Product>
                (
                    new CommandDefinition
                    ("""
                        SELECT * FROM products WHERE slug = @slug
                    """, new { slug })
                );

            if (product == null)
            {
                return null;
            }

            var categories = await connection.QueryAsync<string>
                (
                    new CommandDefinition
                    ("""
                        SELECT * FROM categories WHERE productId = @id
                    """, new { id = product.Id })
                );

            foreach (var category in categories)
            {
                product.Categories.Add(category);
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync();

            var result = await connection.QueryAsync
                (
                    new CommandDefinition
                    ("""
                        SELECT p.*, STRING_AGG(c.name, ',') AS categories
                        FROM products p
                        LEFT JOIN categories c ON p.id = c.productId
                        GROUP BY p.id, p.name, p.brand, p.slug, p.amount, p.description
                    """)
                );

            return result.Select(p => new Product
            {
                Id = p.id,
                Name = p.name,
                Brand = p.brand,
                Ammount = p.amount,
                Categories = Enumerable.ToList(p.categories.Split(',')),
                Description = p.description,
            });

        }

        public Task<bool> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsProductAsync(Guid id)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync();

            return await connection.ExecuteScalarAsync<bool>(new CommandDefinition("""
                SELECT COUNT(1) FROM products WHERE id = @id
                """, new { id }));
        }
    }
}