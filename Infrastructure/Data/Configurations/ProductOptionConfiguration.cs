using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class ProductOptionConfiguration : IEntityTypeConfiguration<ProductOption>
    {
        public void Configure(EntityTypeBuilder<ProductOption> builder)
        {
            builder.Property(o => o.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

            builder.Property(o => o.Stock)
                    .IsRequired();

            builder.HasOne(o => o.Product)
                    .WithMany(p => p.Options)
                    .HasForeignKey(o => o.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
