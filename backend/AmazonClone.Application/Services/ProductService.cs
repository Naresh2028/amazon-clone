using AmazonClone.Application.DTOs;
using AmazonClone.Application.Interfaces;
using AmazonClone.Domain.Entities;
using AmazonClone.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository
            ) 
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            List<Product> items = await _productRepository.GetAllAsync();

            List<ProductResponseDto> resultItems = new List<ProductResponseDto>();

            for (int i = 0; i < items.Count ; i++)
            {
                resultItems.Add(ProductEntityMapToDto(items[i]));
            }

            return resultItems;
        }

        public async Task<ProductResponseDto?> GetByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product is null) 
            {
                return null;
            }

            return ProductEntityMapToDto(product);
        }
        public async Task<ProductResponseDto> CreateAsync(CreateProductRequestDto request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category is null) 
            {
                throw new DomainException("Category does not exist!");
            }

            Product product = new Product 
            {
                Id = Guid.NewGuid(),
                Name  = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = request?.ImageUrl?.Trim(),
                IsActive = true,
                CategoryId = category.Id,
                Category = category
            };

            await _productRepository.AddAsync(product);

            return ProductEntityMapToDto(product);

        }


        public async Task<ProductResponseDto?> UpdateAsync(Guid productId, UpdateProductRequestDto request)
        {
            Product? product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return null;
            }

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category is null)
            {
                throw new DomainException("Category does not exist!");
            }

            // Update properties directly on the tracked entity
            product.Name = request.Name.Trim();
            product.Description = request.Description?.Trim();
            product.Price = request.Price;
            product.StockQuantity = request.StockQuantity;
            product.ModifiedAt = DateTime.UtcNow;
            product.ImageUrl = request.ImageUrl?.Trim();
            product.IsActive = request.IsActive;
            product.CategoryId = category.Id;
            product.Category = category;

            await _productRepository.UpdateAsync(product);

            return ProductEntityMapToDto(product);
        }


        public async Task<bool> DeleteAsync(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId: productId);

            if (product == null) 
            {
                return false;
            }

            await _productRepository.DeleteAsync(product);
            return true;
        }


        private static ProductResponseDto ProductEntityMapToDto(Product product)
        {

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name.Trim(),
                Description = product.Description.Trim(),
                Price = product.Price,
                CategoryId = product.CategoryId,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CategoryName = product.Category?.Name ?? string.Empty
            };
        }
    }
}
