using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Stock
{
    public class GetStockDto
    {
        public int ProductOptionId { get; set; }
        public string ProductTitle { get; set; }
        public string Size { get; set; }
        public int AvailableQuantity { get; set; }
        public int ReservedQuantity { get; set; }
    }
}
