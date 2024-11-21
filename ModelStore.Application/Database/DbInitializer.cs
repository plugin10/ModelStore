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

            ///create product table
            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='product' AND xtype='U')
                CREATE TABLE product (
                    id UNIQUEIDENTIFIER PRIMARY KEY,
                    slug NVARCHAR(255) NOT NULL,
                    name NVARCHAR(255) NOT NULL,
                    brand NVARCHAR(255) NOT NULL,
                    price DECIMAL(10, 2) NOT NULL,
                    stock INT,
                    description NVARCHAR(MAX)
                );
            ");

            ///create index for product table
            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'product_slug_idx')
                CREATE UNIQUE INDEX product_slug_idx
                ON product (slug);
            ");

            ///create category table
            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='categorie' AND xtype='U')
                CREATE TABLE categorie (
                    id INT IDENTITY(1,1) PRIMARY KEY,
                    product_id UNIQUEIDENTIFIER NOT NULL,
                    name NVARCHAR(255) NOT NULL,
                    CONSTRAINT FK_Categories_Products FOREIGN KEY (product_id) REFERENCES product(id)
                );
            ");

            ///create user table
            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='user' AND xtype='U')
                    CREATE TABLE [user] (
                        id INT PRIMARY KEY IDENTITY(1,1),
                        name NVARCHAR(100) NOT NULL,
                        email NVARCHAR(100) UNIQUE NOT NULL,
                        password_hash NVARCHAR(256) NOT NULL,
                        user_type NVARCHAR(50) CHECK (user_type IN ('Client', 'Employee', 'Administrator')) NOT NULL
                    );
            ");

            ///create rating table
            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='rating' AND xtype='U')
                    CREATE TABLE rating (
                        rating_id INT PRIMARY KEY IDENTITY(1,1),
                        user_id INT NOT NULL,
                        product_id UNIQUEIDENTIFIER NOT NULL,
                        rating INT CHECK (Rating BETWEEN 1 AND 5),
                        created_at DATETIME DEFAULT GETDATE(),
                        FOREIGN KEY (user_id) REFERENCES [user](id),
                        FOREIGN KEY (product_id) REFERENCES product(id)
                    );
            ");
        }
    }
}