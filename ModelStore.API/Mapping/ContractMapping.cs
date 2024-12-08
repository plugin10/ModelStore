﻿using ModelStore.Application.Models;
using ModelStore.Contracts.Requests;
using ModelStore.Contracts.Responses;

namespace ModelStore.API.Mapping
{
    public static class ContractMapping
    {
        public static Product MapToGood(this CreateProductRequest request)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Brand = request.Brand,
                Price = request.Price,
                Stock = request.Stock,
                Categories = request.Categories.ToList(),
                Description = request.Description,
            };
        }

        public static Product MapToGood(this UpdateProductRequest request, Guid id)
        {
            return new Product
            {
                Id = id,
                Name = request.Name,
                Brand = request.Brand,
                Price = request.Price,
                Stock = request.Stock,
                Categories = request.Categories.ToList(),
                Description = request.Description,
            };
        }

        public static ProductResponse MapToResponse(this Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Brand = product.Brand,
                Slug = product.Slug,
                Rating = product.Rating,
                Price = product.Price,
                Stock = product.Stock,
                Categories = product.Categories,
                Description = product.Description
            };
        }

        public static ProductsResponse MapToResponse(this IEnumerable<Product> products)
        {
            return new ProductsResponse
            {
                Items = products.Select(MapToResponse),
            };
        }
    }
}