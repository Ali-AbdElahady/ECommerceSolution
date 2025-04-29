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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Application.Common.Behaviours;

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

            Services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
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
                });

            Services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

            Services.AddAuthorization();
            return Services;
        }
    }
}
