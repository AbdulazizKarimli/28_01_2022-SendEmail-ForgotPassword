using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.Models;

namespace FrontToBack.ViewModels
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }
     
        public List<Product> Products { get; set; }
    }
}
