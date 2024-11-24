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
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _DBconnectionFactory;

        public UserRepository(IDbConnectionFactory dBconnectionFactory)
        {
            _DBconnectionFactory = dBconnectionFactory;
        }

        public async Task<User?> GetByEmailAndPasswordAsync(string email, string password, CancellationToken token = default)
        {
            using var connection = await _DBconnectionFactory.CreateConnectionAsync(token);
            var user = await connection.QuerySingleOrDefaultAsync<User>
                (
                    new CommandDefinition
                    ("""
                        SELECT * FROM [user] WHERE email = @email AND password_hash = @password
                    """, new { email, password },
                    cancellationToken: token)
                );

            return user;
        }
    }
}