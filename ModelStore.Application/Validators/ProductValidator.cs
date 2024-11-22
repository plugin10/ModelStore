using FluentValidation;
using ModelStore.Application.Models;
using ModelStore.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelStore.Application.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        private readonly IProductRepository _productRepository;

        public ProductValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Categories).NotEmpty();
            RuleFor(p => p.Brand).NotEmpty();
            RuleFor(p => p.Price).GreaterThan(0);
            RuleFor(p => p.Stock).GreaterThanOrEqualTo(0);
            RuleFor(p => p.Slug).MustAsync(ValidateSlug).WithMessage("Ten produkt został już dodany");
        }

        private async Task<bool> ValidateSlug(Product product, string slug, CancellationToken token = default)
        {
            var existingProduct = await _productRepository.GetBySlugAsync(product.Slug);

            if (existingProduct != null)
            {
                return existingProduct.Id == product.Id;
            }

            return existingProduct == null;
        }
    }
}