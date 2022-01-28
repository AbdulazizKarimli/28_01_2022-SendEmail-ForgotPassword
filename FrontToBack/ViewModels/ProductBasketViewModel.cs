using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.ViewModels
{
    public class ProductBasketViewModel
    {
        public List<ProductItemBasketViewModel> ProductItemBasketViewModels { get; set; }

        public int TotalPrice { get; set; }
    }
}
