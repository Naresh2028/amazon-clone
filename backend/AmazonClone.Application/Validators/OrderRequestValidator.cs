using AmazonClone.Application.DTOs;
using AmazonClone.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.Validators
{
    public class OrderRequestValidator : AbstractValidator<OrderItemRequestDto>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product is required");

            RuleFor(x => x.Quantity)
                .InclusiveBetween(0, 100)
                .WithMessage("Quantity must be between 1 and 100");
        }
    }
}
