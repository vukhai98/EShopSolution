using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { set; get; }
        public decimal Price { set; get; }
        public decimal OriginalPrice { set; get; } //Giá gốc
        public int Stock { set; get; }  //hàng hóa
        public int ViewCount { set; get; }
        public DateTime DataCreated { set; get; }
        public string SeoAlias { set; get; }
    }
}
