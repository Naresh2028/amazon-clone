using AmazonClone.Application.Common;
using AmazonClone.Application.DTOs;
using AmazonClone.Application.Interfaces;
using AmazonClone.Domain.Entities;
using AmazonClone.Domain.Exceptions;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;
        private readonly ICacheService _cacheService;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ICacheService cacheService,
            ILogger<CategoryService> logger
            )
        {
            _categoryRepository = categoryRepository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {

            List<CategoryResponseDto>? cachedCategories = await _cacheService.GetAsync<List<CategoryResponseDto>>(CacheKeys.CategoryList);

            if (cachedCategories != null) 
            {
                _logger.LogInformation("Category List Served from Redis Server");
                return cachedCategories;
            }

            _logger.LogInformation("Category List loaded from Database");

            List<Category> categories = await _categoryRepository.GetAllAsync();

            List<CategoryResponseDto> result = new List<CategoryResponseDto>();

            for (int i = 0; i < categories.Count; i++)
            {
                result.Add(MapToDto(categories[i]));
            }

            await _cacheService.SetAsync(CacheKeys.CategoryList, result, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2));

            return result;
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(Guid id)
        {

            CategoryResponseDto? cachedCategory = await _cacheService
                            .GetAsync<CategoryResponseDto>(CacheKeys.CategoryById(id));

            if (cachedCategory != null) 
            {
                _logger.LogInformation("Category Id served from Redis");
                return cachedCategory;
            }

            _logger.LogInformation("Category Id loaded from Database");

            Category? category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return null;
            }
            
            CategoryResponseDto dto = MapToDto(category);

            await _cacheService.SetAsync(CacheKeys.CategoryById(id), dto, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(2));

            return dto;
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto request)
        {

            _logger.LogInformation("Creating Category {CategoryName}", request.Name);

            bool exists = await _categoryRepository.NameExistsAsync(request.Name, Guid.Empty);

            if (exists)
            {
                throw new DomainException("A category with this name already exists.");
            }

            Category category = new Category();
            category.Id = Guid.NewGuid();
            category.Name = request.Name.Trim();
            category.Description = request.Description.Trim();
            category.IsActive = true;

            _logger.LogInformation("Category {CategoryId} Created", category.Id);

            await _categoryRepository.AddAsync(category);

            await _cacheService.RemoveAsync(CacheKeys.CategoryList);
            return MapToDto(category);
        }

        public async Task<CategoryResponseDto?> UpdateAsync(Guid id, UpdateCategoryRequestDto request)
        {
            Category? category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }

            bool exists = await _categoryRepository.NameExistsAsync(request.Name, id);
            if (exists)
            {
                throw new DomainException("A category with this name already exists.");
            }

            category.Name = request.Name.Trim();
            category.Description = request.Description.Trim();

            await _categoryRepository.UpdateAsync(category);

            await _cacheService.RemoveAsync(CacheKeys.CategoryList);
            await _cacheService.RemoveAsync(CacheKeys.CategoryById(id));

            return MapToDto(category);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Category? category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            await _categoryRepository.DeleteAsync(category);

            await _cacheService.RemoveAsync(CacheKeys.CategoryList);
            await _cacheService.RemoveAsync(CacheKeys.CategoryById(id));

            return true;
        }

        private static CategoryResponseDto MapToDto(Category category)
        {
            CategoryResponseDto dto = new CategoryResponseDto();
            dto.Id = category.Id;
            dto.Name = category.Name;
            dto.Description = category.Description;
            dto.IsActive = category.IsActive;
            return dto;
        }
    }
}
