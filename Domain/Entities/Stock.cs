using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Stock
    {
        public int Id { get; set; }

        public int ProductOptionId { get; set; }
        public ProductOption ProductOption { get; set; }

        public int Quantity { get; set; }
        public int Reserved { get; set; }
    }
}
