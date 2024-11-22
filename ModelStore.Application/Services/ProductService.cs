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

        public async Task<bool> CreateAsync(Product product)
        {
            await _productValidator.ValidateAndThrowAsync(product);
            return await _productRepository.CreateAsync(product);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return _productRepository.GetAllAsync();
        }

        public Task<Product?> GetByIdAsync(Guid id)
        {
            return _productRepository.GetByIdAsync(id);
        }

        public Task<Product?> GetBySlugAsync(string slug)
        {
            return _productRepository.GetBySlugAsync(slug);
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            await _productValidator.ValidateAndThrowAsync(product);
            var productExist = await _productRepository.ExistsProductAsync(product.Id);

            if (!productExist)
            {
                return null;
            }

            await _productRepository.UpdateProductAsync(product);
            return product;
        }

        public Task<bool> DeleteProductAsync(Guid id)
        {
            return _productRepository.DeleteProductAsync(id);
        }
    }
}