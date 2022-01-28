using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.ViewModels
{
    public class BasketViewModel
    {
        public int ProductId { get; set; }

        public int Count { get; set; } = 1;
    }
}
