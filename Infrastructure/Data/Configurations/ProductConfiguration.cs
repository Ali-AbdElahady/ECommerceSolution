using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(p => p.ProductCategory)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.ProductCategoryId);
            builder.HasMany(p => p.Images)
                    .WithOne(i => i.Product)
                    .HasForeignKey(i => i.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(p => p.Options)
                    .WithOne(o => o.Product)
                    .HasForeignKey(o => o.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
