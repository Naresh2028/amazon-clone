using System;
using FluentValidation;
using System.Collections.Generic;
using System.Text;
using AmazonClone.Application.DTOs;

namespace AmazonClone.Application.Validators
{
    public class PlaceOrderRequestValidator : AbstractValidator<PlaceOrderRequestDto>
    {
        public PlaceOrderRequestValidator() 
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(80).WithMessage("Name should be less than 80 characters");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .Matches("^[0-9]{10}$").WithMessage("Phone must be 10 digits");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MinimumLength(10).WithMessage("Address should be at least 10 characters")
                .MaximumLength(100).WithMessage("Address can not exceeds more than 100 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City must be required")
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters.");

            RuleFor(x => x.Pincode)
                .NotEmpty().WithMessage("Pincode is required")
                .Matches("^[0-9]{6}$").WithMessage("Pincode must be exactly 6 digits.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required.")
                .Must(HaveNoDuplicateProducts).WithMessage("The same product cannot appear twice.");

        }

        private static bool HaveNoDuplicateProducts(List<OrderItemRequestDto> items) 
        {
            HashSet<Guid> seenProducts = new HashSet<Guid>();
            

            for (int i = 0; i < items.Count ; i++)
            {

                if (seenProducts.Contains(items[i].ProductId)) 
                {
                    return false;
                }

                seenProducts.Add(items[i].ProductId);
            }
                
            return true;
        }
    }
}
