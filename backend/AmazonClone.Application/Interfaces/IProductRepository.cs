using AmazonClone.Application.DTOs;
using AmazonClone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid productId);
        Task<bool> ExistInCategory(Guid CategoryId);
        Task AddAsync(Product request);
        Task UpdateAsync(Product request);
        Task DeleteAsync(Product request);
    }
}
