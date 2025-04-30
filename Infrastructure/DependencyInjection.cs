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
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.AddScoped<IIdentityService, IdentityService>();
            Services.AddScoped<ApplicationDbContextInitialiser>();
            Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
                                                                configuration.GetConnectionString("DefaultConnection"),
                                                                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

            Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            Services.AddValidatorsFromAssemblyContaining<AddProductCommandValidator>();
            Services.AddValidatorsFromAssemblyContaining<ConfirmShippingCommandValidator>();

            Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(GetAllOrdersQueryHandler).Assembly,
                    typeof(ConfirmShippingCommandHandler).Assembly,
                    typeof(AddProductCommandHandler).Assembly,
                    typeof(GetProductByIdQueryHandler).Assembly
                );

                // Add all the pipeline behaviors
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            });



            Services.AddAuthentication(options =>
                                                {
                                                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                                })
                .AddJwtBearer(options =>
                {
                    var jwtSettings = configuration.GetSection("JwtSettings");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult(); // Stop the default behavior

                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";

                            var result = JsonSerializer.Serialize(new { message = "Invalid or expired token." });
                            return context.Response.WriteAsync(result);
                        }
                    };
                });

            Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            Services.AddAuthorization();
            return Services;
        }
    }
}
