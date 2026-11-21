using AmazonClone.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Domain.Entities
{
    public class ShippingDetails : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
