using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Database
{
    public class DbInitializer
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DbInitializer(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task InitializerAsync()
        {
            using var connection = await _dbConnectionFactory.CreateConnectionAsync();

            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='products' AND xtype='U')
                CREATE TABLE products (
                    id UNIQUEIDENTIFIER PRIMARY KEY,
                    slug NVARCHAR(255) NOT NULL,
                    name NVARCHAR(255) NOT NULL,
                    brand NVARCHAR(255) NOT NULL,
                    amount INT NOT NULL,
                    description NVARCHAR(MAX)
                );
            ");

            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'products_slug_idx')
                CREATE UNIQUE INDEX products_slug_idx
                ON products (slug);
            ");

            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='categories' AND xtype='U')
                CREATE TABLE categories (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    productId UNIQUEIDENTIFIER NOT NULL,
                    name NVARCHAR(255) NOT NULL,
                    CONSTRAINT FK_Categories_Products FOREIGN KEY (productId) REFERENCES products(id)
                );
            ");
        }
    }
}