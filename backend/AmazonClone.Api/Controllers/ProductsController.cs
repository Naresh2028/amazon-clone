using AmazonClone.Application.DTOs;
using AmazonClone.Application.Interfaces;
using AmazonClone.Application.Validators;
using AmazonClone.Domain.Entities;
using AmazonClone.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazonClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidator<CreateProductRequestDto> _createValidator;
        private readonly IValidator<UpdateProductRequestDto> _updateValidator;

        public ProductsController(
                IProductService productService,
                IValidator<CreateProductRequestDto> createValidator,
                IValidator<UpdateProductRequestDto> updateValidator
            ) 
        {
            _productService = productService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() 
        {
            List<ProductResponseDto> products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{productId:Guid}")]
        [ActionName(nameof(GetByIdAsync))]
        public async Task<IActionResult> GetByIdAsync(Guid productId) 
        {
            ProductResponseDto? product = await _productService.GetByIdAsync(productId);

            if (product == null) return NotFound();

            return Ok(product);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateProductRequestDto request) 
        {
            ValidationResult validationResult = await _createValidator.ValidateAsync(request);

            if (!validationResult.IsValid) 
            {
                return ValidationProblemFrom(validationResult);
            }


            ProductResponseDto productCreated =  await _productService.CreateAsync(request);
            return CreatedAtAction(nameof(GetByIdAsync), new { productId = productCreated.Id }, productCreated);
            
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateProductRequestDto request)
        {
            ValidationResult validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return ValidationProblemFrom(validationResult);
            }

            ProductResponseDto? updated = await _productService.UpdateAsync(id, request);

            if (updated == null)
            {
                return NotFound();
            }
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            bool deleted = await _productService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        private IActionResult ValidationProblemFrom(ValidationResult validationResult) 
        {
            foreach (ValidationFailure failure in validationResult.Errors)
            {
                ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
            }
            
            return ValidationProblem(ModelState);
        }
    }
}
