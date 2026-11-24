using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.DTOs
{
    public class OrderItemRequestDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
