using AmazonClone.Domain.Entities;
using AmazonClone.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        Task<bool> TryUpdateStatusAsync(Guid id, OrderStatus status, OrderStatus newStatus);
    }
}
