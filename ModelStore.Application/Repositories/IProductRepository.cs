﻿using ModelStore.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Repositories
{
    public interface IProductRepository
    {
        Task<bool> CreateAsync(Product product, CancellationToken token = default);

        Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);

        Task<Product?> GetBySlugAsync(string slug, CancellationToken token = default);

        Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default);

        Task<bool> UpdateProductAsync(Product product, CancellationToken token = default);

        Task<bool> DeleteProductAsync(Guid id, CancellationToken token = default);

        Task<bool> ExistsProductAsync(Guid id, CancellationToken token = default);
    }
}