using Domain.Constants;
using ECommerceSolution.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;


namespace Infrastructure.Data
{
    public static class InitialiserExtensions
    {

        public static async Task InitialiseDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

            await initialiser.InitialiseAsync();
            await initialiser.SeedAsync();
        }
    }
    public class ApplicationDbContextInitialiser
    {
        private readonly ILogger<ApplicationDbContextInitialiser> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedCategoriesAsync();
            await SeedProductsAsync();
            await SeedStockAsync();
            await SeedOrdersAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roles = new[] { Roles.Administrator, Roles.SalesManager, Roles.InventoryManager, Roles.Client };
            foreach (var roleName in roles)
            {
                if (_roleManager.Roles.All(r => r.Name != roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            var administrator = new ApplicationUser
            {
                UserName = "administrator@localhost",
                Email = "administrator@localhost",
                EmailConfirmed = true
            };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "Administrator1!");
                await _userManager.AddToRolesAsync(administrator, new[] { Roles.Administrator });
            }

            var salesManager = new ApplicationUser
            {
                UserName = "SalesManager@localhost",
                Email = "SalesManager@localhost",
                EmailConfirmed = true
            };

            if (_userManager.Users.All(u => u.UserName != salesManager.UserName))
            {
                await _userManager.CreateAsync(salesManager, "SalesManager1!");
                await _userManager.AddToRolesAsync(salesManager, new[] { Roles.SalesManager });
            }

            var inventoryManager = new ApplicationUser
            {
                UserName = "InventoryManager@localhost",
                Email = "InventoryManager@localhost",
                EmailConfirmed = true
            };

            if (_userManager.Users.All(u => u.UserName != inventoryManager.UserName))
            {
                await _userManager.CreateAsync(inventoryManager, "InventoryManager1!");
                await _userManager.AddToRolesAsync(inventoryManager, new[] { Roles.InventoryManager });
            }

            var client = new ApplicationUser
            {
                UserName = "client@localhost",
                Email = "client@localhost",
                EmailConfirmed = true
            };

            if (_userManager.Users.All(u => u.UserName != client.UserName))
            {
                await _userManager.CreateAsync(client, "Client1!");
                await _userManager.AddToRolesAsync(client, new[] { Roles.Client });
            }

        }

        private async Task SeedCategoriesAsync()
        {
            if (!_context.Set<ProductCategory>().Any())
            {
                var categories = new List<ProductCategory>
        {
            new ProductCategory { Name = "Clothing" },
            new ProductCategory { Name = "Footwear" },
            new ProductCategory { Name = "Accessories" }
        };

                _context.Set<ProductCategory>().AddRange(categories);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedProductsAsync()
        {
            if (!_context.Set<Product>().Any())
            {
                var categories = _context.Set<ProductCategory>().ToList();

                var tShirt = new Product
                {
                    Title = "Basic T-Shirt",
                    Description = "Comfortable cotton t-shirt for everyday wear.",
                    ProductCategoryId = categories.First(c => c.Name == "Clothing").Id,
                    Images = new List<Image>
            {
                new Image { ImagePath = "/images/shirt.jpg" }
            },
                    Options = new List<ProductOption>
            {
                new ProductOption { Size = "S", Price = 19.99m  },
                new ProductOption { Size = "M", Price = 19.99m },
                new ProductOption { Size = "L", Price = 19.99m },
                new ProductOption { Size = "XL", Price = 19.99m }
            }
                };

                var sneakers = new Product
                {
                    Title = "Running Sneakers",
                    Description = "Lightweight sneakers for running and casual wear.",
                    ProductCategoryId = categories.First(c => c.Name == "Footwear").Id,
                    Images = new List<Image>
            {
                new Image { ImagePath = "/images/Sneakers.jpg" }
            },
                    Options = new List<ProductOption>
            {
                new ProductOption { Size = "40", Price = 79.99m },
                new ProductOption { Size = "41", Price = 79.99m },
                new ProductOption { Size = "42", Price = 79.99m },
                new ProductOption { Size = "43", Price = 79.99m }
            }
                };

                var backpack = new Product
                {
                    Title = "Classic Backpack",
                    Description = "Durable backpack for everyday use and travel.",
                    ProductCategoryId = categories.First(c => c.Name == "Accessories").Id,
                    Images = new List<Image>
            {
                new Image { ImagePath = "/images/Backpack.jpg" }
            },
                    Options = new List<ProductOption>
            {
                new ProductOption { Size = "One Size", Price = 49.99m }
            }
                };

                _context.Set<Product>().AddRange(tShirt, sneakers, backpack);
                await _context.SaveChangesAsync();
            }
        }
        private async Task SeedStockAsync()
        {
            if (!_context.Set<Stock>().Any())
            {
                var productOptions = _context.Set<ProductOption>().ToList();

                var stockList = productOptions.Select(option => new Stock
                {
                    ProductOptionId = option.Id,
                    Quantity = 100,      // Example default quantity
                    Reserved = 0         // Initially nothing reserved
                }).ToList();

                _context.Set<Stock>().AddRange(stockList);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedOrdersAsync()
        {
            if (!_context.Set<Order>().Any())
            {
                var clientUser = await _userManager.FindByEmailAsync("client@localhost");
                var products = _context.Set<Product>().Include(p => p.Options).ToList();

                if (clientUser != null && products.Any())
                {
                    var order = new Order
                    {
                        OrderDate = DateTime.UtcNow.AddDays(-3),
                        CustomerId = clientUser.Id,
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                ProductOptionId = products[0].Options.First().Id,
                                ProductId = products[0].Id,
                                Quantity = 2,
                                Price = products[0].Options.First().Price
                            },
                            new OrderItem
                            {
                                ProductOptionId = products[1].Options.First().Id,
                                ProductId = products[1].Id,
                                Quantity = 1,
                                Price = products[1].Options.First().Price
                            }
                        }
                    };

                    _context.Set<Order>().Add(order);
                    await _context.SaveChangesAsync();
                }
            }


        }
    }
}
