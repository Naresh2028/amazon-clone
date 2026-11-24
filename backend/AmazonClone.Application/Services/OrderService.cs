using AmazonClone.Application.DTOs;
using AmazonClone.Application.Interfaces;
using AmazonClone.Domain.Entities;
using AmazonClone.Domain.Enums;
using AmazonClone.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AmazonClone.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        // Excludes 0, O, 1, I to avoid typos when users read or type the ID
        private const string SAFE_ALPHABET = "23456789ABCDEFGHJKLMNPQRSTUVWXYZ";
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private ILogger<OrderService> _logger;
        public OrderService(
            IUnitOfWork unitOfWork,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ILogger<OrderService> logger
            ) 
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<List<OrderResponseDto>> GetAllAsync() 
        {
            List<Order> OrderList = new List<Order>();

            OrderList = await _orderRepository.GetAllAsync();

            List<OrderResponseDto> result = new List<OrderResponseDto>();

            for (int i = 0; i < OrderList.Count ; i++)
            {
                result.Add(MapToDto(OrderList[i]));
            }

            return result;
        }

        public async Task<OrderResponseDto?> GetByIdAsync(Guid id)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return null;
            }
            return MapToDto(order);
        }
        public async Task<OrderResponseDto> PlaceOrderAsync(PlaceOrderRequestDto request) 
        {
            _logger.LogInformation("Placing an order {ItemCount} items",request.Items.Count);

            List<Guid> productIds = new List<Guid>();

            // 1. Load every product in ONE query
            for (int i = 0; i < request.Items.Count; i++)
            {
                productIds.Add(request.Items[i].ProductId);
            }

            List<Product> products = await _productRepository.GetByIdsAsync(productIds);

            // 2. Build the order. Names and prices come from the DATABASE, never from the client.
            Order order = new Order() 
            {
                OrderNumber = GenerateOrderNumber(),
                Status = OrderStatus.Pending,
                ShippingDetails = new ShippingDetails
                {
                    FullName = request.FullName.Trim(),
                    Address = request.Address.Trim(),
                    Mobile = request.Phone.Trim(),
                    City = request.City.Trim(),
                    Pincode = request.Pincode.Trim()
                }
            };

            decimal totalAmount = 0;

            for (int i = 0; i < request.Items.Count; i++) 
            {
                OrderItemRequestDto requestItem = request.Items[i];

                Product? product = FindProduct(products, requestItem.ProductId);

                if (product == null || !product.IsActive) 
                {
                    throw new  DomainException($"Product {requestItem.ProductId} is not Availbale");
                }

                OrderItem orderItem = new OrderItem();

                orderItem.ProductId = product.Id;
                orderItem.ProductName = product.Name;
                orderItem.UnitPrice = product.Price;
                orderItem.Quantity = product.StockQuantity;
                orderItem.LineTotal = product.Price * requestItem.Quantity;

                order.Items.Add(orderItem);
                totalAmount = totalAmount + orderItem.LineTotal;
            }

            order.TotalAmount = totalAmount;

            // 3. Reserve stock and save the order in ONE transaction
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                foreach(var item in order.Items)
                {
                    bool isReserved = await _productRepository.TryReduceStockAsync(item.ProductId, item.Quantity);

                    if (!isReserved) 
                    {
                        throw new DomainException($"Insufficiant stock for {item.ProductName}.");
                    }
                }

                await _orderRepository.AddAsync(order);
                await _unitOfWork.CommitAsync();
            }
            catch 
            {

                await _unitOfWork.RollBackAsync();
                throw;
            }

            _logger.LogInformation("Order {OrderNumber} placed with total {TotalAmount}", order.OrderNumber, order.TotalAmount);
            return MapToDto(order);
        }

        public async Task<OrderResponseDto?> CancelOrderAsync(Guid id)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return null;
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                bool isChanged = await _orderRepository.TryUpdateStatusAsync(
                    id, OrderStatus.Pending, OrderStatus.Cancelled);

                if (!isChanged)
                {
                    throw new DomainException("Only pending orders can be cancelled.");
                }

                foreach (var item in order.Items)
                {
                    await _productRepository.RestoreStockAsync(item.ProductId, item.Quantity);
                }

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollBackAsync();
                throw;
            }

            _logger.LogInformation("Order {OrderNumber} cancelled and stock restored", order.OrderNumber);

            Order? updatedOrder = await _orderRepository.GetByIdAsync(id);
            if (updatedOrder == null)
            {
                return null;
            }
            return MapToDto(updatedOrder);
        }
        private static Product? FindProduct(List<Product> products,Guid id) 
        {
            for (int i = 0; i < products.Count; i++) 
            {
                if (products[i].Id == id) 
                {
                    return products[i];
                }
            }
            return null;
        }

        private static string GenerateOrderNumber(int length = 7) 
        {
            string datePart = DateTime.UtcNow.ToString("yyyyMMDD");

            string randomPart = RandomNumberGenerator.GetString(SAFE_ALPHABET,length);

            return $"ORD-{datePart}-{randomPart}";
        }

        private static OrderResponseDto MapToDto(Order order)
        {
            OrderResponseDto dto = new OrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                Items = order.Items?.Select(item => new OrderItemResponseDto
                {
                    ProductId = item.Id,
                    ProductName = item.ProductName,
                    UnitPrice = item.UnitPrice,
                    Quantity = item.Quantity,
                    LineTotoal = item.LineTotal
                }).ToList() ?? new List<OrderItemResponseDto>()
            };

            if (order.ShippingDetails != null)
            {
                dto.ShippingDetails = new ShippingDetails
                {
                    Id = order.ShippingDetails.Id,
                    FullName = order.ShippingDetails.FullName,
                    Mobile = order.ShippingDetails.Mobile,
                    Address = order.ShippingDetails.Address,
                    City = order.ShippingDetails.City,
                    Pincode = order.ShippingDetails.Pincode,
                    OrderId = order.ShippingDetails.OrderId,
                    CreatedAt = order.ShippingDetails.CreatedAt
                };
            }

            return dto;

        }
    }
}
