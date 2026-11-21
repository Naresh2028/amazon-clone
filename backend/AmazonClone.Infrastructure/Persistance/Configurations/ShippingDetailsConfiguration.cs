using AmazonClone.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text;

namespace AmazonClone.Infrastructure.Persistance.Configurations
{
    public class ShippingDetailsConfiguration : IEntityTypeConfiguration<ShippingDetails>
    {
        public void Configure(EntityTypeBuilder<ShippingDetails> modelBuilder) 
        {
            modelBuilder
                .ToTable("shipping_details");

            modelBuilder
                .Property(s => s.FullName)
                .IsRequired()
                .HasMaxLength(80);

            modelBuilder
                .Property(s => s.City)
                .HasMaxLength(50);

            modelBuilder
                .Property(s => s.Address)
                .HasMaxLength(90);

            modelBuilder
                .Property(s => s.Mobile)
                .IsRequired()
                .HasMaxLength(90);

            modelBuilder
                .HasOne(s => s.Order)
                .WithOne(s => s.ShippingDetails)
                .HasForeignKey<ShippingDetails>(s => s.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
