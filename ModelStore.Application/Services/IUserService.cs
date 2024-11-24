using ModelStore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Services
{
    public interface IUserService
    {
        Task<User?> SignInUser(string email, string password);
    }
}