using AmazonClone.Domain.Common;
using AmazonClone.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal TotalAmount { get; set; }
        public ShippingDetails? ShippingDetails { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
