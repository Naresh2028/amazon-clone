using AmazonClone.Application.DTOs;
using AmazonClone.Application.Interfaces;
using AmazonClone.Domain.Entities;
using AmazonClone.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            List<Category> categories = await _categoryRepository.GetAllAsync();

            List<CategoryResponseDto> result = new List<CategoryResponseDto>();
            for (int i = 0; i < categories.Count; i++)
            {
                result.Add(MapToDto(categories[i]));
            }
            return result;
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(Guid id)
        {
            Category? category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return null;
            }
            return MapToDto(category);
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto request)
        {
            bool exists = await _categoryRepository.NameExistsAsync(request.Name, Guid.Empty);
            if (exists)
            {
                throw new DomainException("A category with this name already exists.");
            }

            Category category = new Category();
            category.Name = request.Name.Trim();
            category.Description = request.Description.Trim();
            category.IsActive = true;

            await _categoryRepository.AddAsync(category);
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
