using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configurations
{
    public class StockConfiguration : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            builder.Property(s => s.Quantity)
                   .IsRequired();

            builder.Property(s => s.Reserved)
                   .IsRequired();

            builder.HasOne(s => s.ProductOption)
                   .WithOne(po => po.Stock)
                   .HasForeignKey<Stock>(s => s.ProductOptionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
