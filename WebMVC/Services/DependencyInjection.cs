using Application.Common.Interfaces;
using Application.Order.Commands;
using Application.Order.Queries;
using Application.Products.Commands.AddProduct;
using Application.Products.Queries;
using Application.Products.Queries.GetProductById;
using FluentValidation;
using Microsoft.OpenApi.Models;

namespace Web.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection Services)
        {
            Services.AddScoped<IUser, CurrentUser>();
            Services.AddScoped<IFileService, FileService>();
            
            



            return Services;
        }
    }
}
