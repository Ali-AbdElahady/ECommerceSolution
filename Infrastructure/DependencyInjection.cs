using Application.Common.Interfaces;
using ECommerceSolution.Infrastructure.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Application.Common.Behaviours;
using Application.Order.Commands;
using Application.Order.Queries;
using Application.Products.Commands.AddProduct;
using Application.Products.Queries;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Application.Srock.Commands.AddStock;
using Application.Srock.Commands.ReleaseStock;
using Application.Srock.Commands.ReserveStock;
using Application.Srock.Commands.UpdateStock;
using Application.Srock.Commands.RedeceStock;
using Application.Srock.Queries;
using Application.Products.Queries.GetProducts;
using Application.Orders.Commands.CreateOrder;
using Application.Interfaces;
using Infrastructure.Services;
using Application.Products.Commands.UpdateProduct;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IIdentityService, IdentityService>();
            Services.AddScoped<ApplicationDbContextInitialiser>();
            Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
                                                                configuration.GetConnectionString("DefaultConnection"),
                                                                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

            

            Services.AddValidatorsFromAssemblyContaining<AddProductCommandValidator>();
            Services.AddValidatorsFromAssemblyContaining<ConfirmShippingCommandValidator>();
            Services.AddValidatorsFromAssemblyContaining<UpdateStockCommandValidator>();
            Services.AddValidatorsFromAssemblyContaining<ReserveStockCommandValidator>();
            Services.AddValidatorsFromAssemblyContaining<ReleaseStockCommandValidator>();
            Services.AddValidatorsFromAssemblyContaining<AddStockCommandValidator>();
            Services.AddValidatorsFromAssemblyContaining<ReduceStockCommandValidator>();
            Services.AddValidatorsFromAssemblyContaining<ProductFilterDtoValidator>();
            Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();




            Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(GetAllOrdersQueryHandler).Assembly,
                    typeof(GetProductByIdQueryHandler).Assembly,
                    typeof(ConfirmShippingCommandHandler).Assembly,
                    typeof(AddProductCommandHandler).Assembly,
                    typeof(AddStockCommandHandler).Assembly,
                    typeof(ReserveStockCommandHandler).Assembly,
                    typeof(UpdateStockCommandHandler).Assembly,
                    typeof(ReleaseStockCommandHandler).Assembly,
                    typeof(ReduceStockCommandHandler).Assembly,
                    typeof(GetStockQuery).Assembly,
                    typeof(ProductsQueryHandler).Assembly,
                    typeof(CreateOrderCommandHandler).Assembly,
                    typeof(ReserveStockCommandHandler).Assembly,
                    typeof(UpdateProductCommandHandler).Assembly





                );

                // Add all the pipeline behaviors
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            });


            #region for JWT Add Authentication
            //Services.AddAuthentication(options =>
            //                                    {
            //                                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //                                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //                                    })
            //    .AddJwtBearer(options =>
            //    {
            //        var jwtSettings = configuration.GetSection("JwtSettings");
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = jwtSettings["Issuer"],
            //            ValidAudience = jwtSettings["Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
            //        };

            //        options.Events = new JwtBearerEvents
            //        {
            //            OnAuthenticationFailed = context =>
            //            {
            //                context.NoResult(); // Stop the default behavior

            //                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            //                context.Response.ContentType = "application/json";

            //                var result = JsonSerializer.Serialize(new { message = "Invalid or expired token." });
            //                return context.Response.WriteAsync(result);
            //            }
            //        };
            //    });
            #endregion

            #region for Identity Authentication
            Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            //Services.AddAuthentication(options =>
            //{
            //    // Set the default authentication scheme to cookies for Identity
            //    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            //    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;

            //    // Add JWT as an additional scheme for API requests
            //    options.DefaultScheme = IdentityConstants.ApplicationScheme;
            //})
            //.AddCookie(IdentityConstants.ApplicationScheme, options =>
            //{
            //    options.LoginPath = "/Identity/Account/Login";
            //    options.LogoutPath = "/Identity/Account/Logout";
            //    options.ExpireTimeSpan = TimeSpan.FromDays(14);
            //    options.SlidingExpiration = true;
            //});

            Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "EcommerceMVC";
            });

            Services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("CLient", policy => policy.RequireRole("CLient"));
                options.AddPolicy("SalesManager", policy => policy.RequireRole("SalesManager"));
                options.AddPolicy("InventoryManager", policy => policy.RequireRole("InventoryManager"));

            });
            #endregion

            FirebaseInitializer.Initialize(configuration);
            Services.AddScoped<INotificationService, FirebaseNotificationService>();

            Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            
            return Services;
        }
    }
}
