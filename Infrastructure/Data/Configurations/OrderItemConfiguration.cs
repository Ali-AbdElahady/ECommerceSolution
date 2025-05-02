using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.Quantity)
            .IsRequired();

            builder.Property(oi => oi.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(oi => oi.Product)
                .WithMany() 
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(oi => oi.ProductOption)
               .WithMany()
               .HasForeignKey(oi => oi.ProductOptionId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
