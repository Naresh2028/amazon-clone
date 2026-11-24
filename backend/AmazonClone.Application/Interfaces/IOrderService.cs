using AmazonClone.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Interfaces
{
    public interface IOrderService
    {
        public Task<List<OrderResponseDto>> GetAllAsync(); 
        public Task<OrderResponseDto> GetByIdAsync(Guid id);
        public Task<OrderResponseDto> PlaceOrderAsync(PlaceOrderRequestDto request);
        public Task<OrderResponseDto> CancelOrderAsync(Guid id); 
    }
}
