using Application.Common.Interfaces;
using ECommerceSolution.Infrastructure.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.AddScoped<IIdentityService, IdentityService>();

            Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(
                                                                configuration.GetConnectionString("DefaultConnection"),
                                                                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

            Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

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
            Services.AddAuthorization();
            return Services;
        }
    }
}
