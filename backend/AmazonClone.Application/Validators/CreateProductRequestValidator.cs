using System;
using FluentValidation;
using System.Collections.Generic;
using System.Text;
using AmazonClone.Application.DTOs;

namespace AmazonClone.Application.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequestDto>
    {
        public CreateProductRequestValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product Name must be required")
                .MaximumLength(40).WithMessage("Name must not exceed 40 characters");

            RuleFor(x => x.Description)
                .MaximumLength(150).WithMessage("Description cannot exceed 150 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price should be greater than 0")
                .Must(price => decimal.Round(price,2) == price)
                .WithMessage("Price must have at least 2 decimal places");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock quantity cannot be negative!");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("CaregoryId is required.");

        }
    }
}
