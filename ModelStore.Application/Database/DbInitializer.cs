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
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='goods' AND xtype='U')
                CREATE TABLE goods (
                    id UNIQUEIDENTIFIER PRIMARY KEY,
                    slug NVARCHAR(255) NOT NULL,
                    name NVARCHAR(255) NOT NULL,
                    brand NVARCHAR(255) NOT NULL,
                    amount INT NOT NULL,
                    description NVARCHAR(MAX)
                );
            ");

            await connection.ExecuteAsync(@"
                IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'goods_slug_idx')
                CREATE UNIQUE INDEX goods_slug_idx
                ON goods (slug);
            ");
        }
    }
}
