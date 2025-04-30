using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Product
{
    public class AddProductDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int ProductCategoryId { get; set; }
        public List<IFormFile>? Images { get; set; } // List of images as IFormFile
        public List<ProductOptionDto> Options { get; set; } = new List<ProductOptionDto>();
    }
}
