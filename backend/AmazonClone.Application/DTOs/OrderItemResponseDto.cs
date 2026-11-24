using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Application.DTOs
{
    public class OrderItemResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotoal { get; set; }
    }
}
