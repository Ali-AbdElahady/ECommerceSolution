using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProdcutId { get; set; }
        public Product Product { get; set; }
    }
}
