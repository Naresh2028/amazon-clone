using AmazonClone.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }

        // Navigation
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
