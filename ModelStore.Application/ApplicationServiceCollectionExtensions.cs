using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ModelStore.Application.Database;
using ModelStore.Application.Repositories;
using ModelStore.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IUserService, UserService>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
            services.AddSingleton<TokenGenerator>();
            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services,
            string connectionString)
        {
            services.AddSingleton<IDbConnectionFactory>(_ =>
                new SqlConnectionFactory(connectionString));
            services.AddSingleton<DbInitializer>();
            return services;
        }
    }
}