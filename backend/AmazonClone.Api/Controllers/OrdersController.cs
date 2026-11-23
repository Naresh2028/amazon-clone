using AmazonClone.Application.DTOs;
using AmazonClone.Application.Interfaces;
using AmazonClone.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazonClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<PlaceOrderRequestDto> _placeOrderValidator;
        public OrdersController(
            IOrderService orderService,
            IValidator<PlaceOrderRequestDto> placeOrderValidator
            ) 
        {
            _orderService = orderService;
            _placeOrderValidator = placeOrderValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() 
        {
            List<OrderResponseDto> result = await _orderService.GetAllAsync();
            
            return Ok(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id) 
        {
            OrderResponseDto? result = await _orderService.GetByIdAsync(id);

            if (result == null) 
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrderAsync(PlaceOrderRequestDto request) 
        {
            await _placeOrderValidator.ValidateAndThrowAsync(request);

            OrderResponseDto created = await _orderService.PlaceOrderAsync(request);

            return CreatedAtAction(nameof(GetByIdAsync),new { id = created.Id },created);
        }

        [HttpPost("{id:Guid}/cancel")]
        public async Task<IActionResult> CancelAsync(Guid id) 
        {
            OrderResponseDto? cancelled = await _orderService.CancelOrderAsync(id);

            if (cancelled == null) 
            {
                return NotFound();
            }
            return Ok(cancelled);
        }
    }
}
