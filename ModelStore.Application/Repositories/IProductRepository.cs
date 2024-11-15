using ModelStore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Repositories
{
    public interface IProductRepository
    {
        Task<bool> CreateAsync(Product product);

        Task<Product?> GetByIdAsync(Guid id);

        Task<Product?> GetBySlugAsync(string slug);

        Task<IEnumerable<Product>> GetAllAsync();

        Task<bool> UpdateProductAsync(Product product);

        Task<bool> DeleteProductAsync(Guid id);

        Task<bool> ExistsProductAsync(Guid id);
    }
}