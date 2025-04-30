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
        public List<ProductOptionDto> Options { get; set; } = new();
    }
}
