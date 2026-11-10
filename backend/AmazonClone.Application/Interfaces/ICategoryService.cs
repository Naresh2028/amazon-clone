using AmazonClone.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllAsync();
        Task<CategoryResponseDto?> GetByIdAsync(Guid id);
        Task<CategoryResponseDto> CreateAsync(CreateCategoryRequestDto request);
        Task<CategoryResponseDto?> UpdateAsync(Guid id, UpdateCategoryRequestDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}
