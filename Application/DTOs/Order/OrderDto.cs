using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public CustomerDto Customer { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public bool IsShipped { get; set; }
    }
}
