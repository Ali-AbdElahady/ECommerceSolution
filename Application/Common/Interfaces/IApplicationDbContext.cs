using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Domain.Entities.Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Stock> Stock { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
