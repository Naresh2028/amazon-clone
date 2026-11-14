using Amazon.Infrastructure.Persistance;
using AmazonClone.Application.Interfaces;
using AmazonClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid productId)
        {
            return await _dbContext.Products
                .Include(p => p.Category)    
                .FirstOrDefaultAsync(p => p.Id == productId);
        }
        public async Task<bool> ExistInCategory(Guid CategoryId)
        {
            return await _dbContext.Products.AnyAsync(p => p.CategoryId == CategoryId);
        }
        public async Task AddAsync(Product request)
        {
            _dbContext.Products.Add(request);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product request)
        {
            _dbContext.Products.Update(request);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Product request)
        {
            _dbContext.Products.Remove(request);
            await _dbContext.SaveChangesAsync();
        }

        
    }
}
