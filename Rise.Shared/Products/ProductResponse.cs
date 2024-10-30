using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Shared.Products
{
    public class ProductResponse
    {
        public IEnumerable<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public int TotalPages { get; set; }
    }
}
