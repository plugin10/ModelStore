﻿using ModelStore.Application.Models;
using ModelStore.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        //private readonly TokenGenerator _tokenGenerator;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User?> SignInUser(string email, string password)
        {
            var user = _userRepository.GetByEmailAndPasswordAsync(email, password);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}