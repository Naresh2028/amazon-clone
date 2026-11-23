using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using AmazonClone.Domain.Entities;

namespace AmazonClone.Infrastructure.Persistance.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> modelBuilder) 
        {
            modelBuilder
                .ToTable("orders");

            modelBuilder
                .HasKey(o => o.Id);

            modelBuilder
                .HasIndex(o => o.TotalAmount)
                .IsUnique();

            modelBuilder
                .Property(o => o.TotalAmount)
                .HasPrecision(18,2);

            modelBuilder
                .Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(20);               

        }
    }
}
