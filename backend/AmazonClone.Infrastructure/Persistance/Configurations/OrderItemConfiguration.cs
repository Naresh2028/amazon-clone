using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;
using AmazonClone.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AmazonClone.Infrastructure.Persistance.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> modelBuilder) 
        {
            modelBuilder
                .ToTable("order_items");

            modelBuilder
                .HasKey(o => o.Id);

            modelBuilder
                .Property(o => o.ProductName)
                .IsRequired()
                .HasMaxLength(80);

            modelBuilder
                .Property(o => o.UnitPrice)
                .HasPrecision(18,2);

            modelBuilder
                .Property(o => o.LineTotal)
                .HasPrecision(18, 2);

            modelBuilder
                .HasOne(o => o.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .HasOne(o => o.Product)
                .WithMany(o => o.Items)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
