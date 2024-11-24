using ModelStore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAndPasswordAsync(string email, string password, CancellationToken token = default);
    }
}