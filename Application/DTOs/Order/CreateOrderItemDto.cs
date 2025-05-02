using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }

        public int OptionId { get; set; }
        public int Quantity { get; set; }
    }
}
