using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
    public class CategoryFilterDto
    {
        public string? Name { get; set; }

        public string? SortBy { get; set; } = "Name"; // Default sort
        public bool SortDescending { get; set; } = false;

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
