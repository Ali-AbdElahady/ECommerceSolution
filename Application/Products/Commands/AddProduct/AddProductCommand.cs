using Application.DTOs.Product;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Products.Commands.AddProduct
{
    public class AddProductCommand : IRequest<int>  
    {
        public AddProductDto Product { get; set; }

        public AddProductCommand(AddProductDto product)
        {
            Product = product;
        }
    }
}
