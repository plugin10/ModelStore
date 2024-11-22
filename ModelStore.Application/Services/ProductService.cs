using FluentValidation;
using ModelStore.Application.Models;
using ModelStore.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IValidator<Product> _productValidator;

        public ProductService(IProductRepository productRepository, IValidator<Product> productValidator)
        {
            _productRepository = productRepository;
            _productValidator = productValidator;
        }

        public async Task<bool> CreateAsync(Product product, CancellationToken token = default)
        {
            await _productValidator.ValidateAndThrowAsync(product, cancellationToken: token);
            return await _productRepository.CreateAsync(product, token);
        }

        public Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default)
        {
            return _productRepository.GetAllAsync(token);
        }

        public Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _productRepository.GetByIdAsync(id, token);
        }

        public Task<Product?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            return _productRepository.GetBySlugAsync(slug, token);
        }

        public async Task<Product?> UpdateProductAsync(Product product, CancellationToken token = default)
        {
            await _productValidator.ValidateAndThrowAsync(product, cancellationToken: token);
            var productExist = await _productRepository.ExistsProductAsync(product.Id, token);

            if (!productExist)
            {
                return null;
            }

            await _productRepository.UpdateProductAsync(product, token);
            return product;
        }

        public Task<bool> DeleteProductAsync(Guid id, CancellationToken token = default)
        {
            return _productRepository.DeleteProductAsync(id, token);
        }
    }
}