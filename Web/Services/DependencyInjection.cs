using Application.Common.Interfaces;
using Application.Order.Commands;
using Application.Order.Queries;
using Application.Products.Commands.AddProduct;
using FluentValidation;
using Microsoft.OpenApi.Models;

namespace Web.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection Services)
        {
            Services.AddScoped<IUser, CurrentUser>();
            Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllOrdersQueryHandler).Assembly));
            Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfirmShippingCommand).Assembly));
            Services.AddValidatorsFromAssembly(typeof(ConfirmShippingCommandValidator).Assembly);
            Services.AddValidatorsFromAssembly(typeof(AddProductCommandValidator).Assembly);


            Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce with Clean Architecture", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token in the format: Bearer {your token here}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            return Services;
        }
    }
}
