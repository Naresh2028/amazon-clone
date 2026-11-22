using Amazon.Infrastructure.Persistance;
using AmazonClone.Application.Interfaces;
using AmazonClone.Domain.Entities;
using AmazonClone.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;
        public OrderRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> GetAllAsync() 
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<Order?> GetById(Guid orderId) 
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == orderId);
        }

        public async Task AddAsync(Order request) 
        {
            _dbContext.Orders.Add(request);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> TryUpdateStatusAsync(Guid id, OrderStatus expectedStatus, OrderStatus newStatus) 
        {
            int updatedRows = await _dbContext.Orders
                .Where(x => x.Id == id &&
                            x.Status == expectedStatus)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Status, newStatus)
                    .SetProperty(x => x.ModifiedAt, DateTime.UtcNow));

            if (updatedRows >= 1) 
            {
                return true;
            }
            return false;
        }
    }
}
