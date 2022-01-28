using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public LayoutService(AppDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public ProductBasketViewModel GetBasket()
        {
            ProductBasketViewModel basketViewModel = new ProductBasketViewModel
            {
                ProductItemBasketViewModels = new List<ProductItemBasketViewModel>(),
                TotalPrice = 0
            };

            string basket = _httpContext.HttpContext.Request.Cookies["basket"];
            List<BasketViewModel> basketProducts;
            if (!string.IsNullOrWhiteSpace(basket))
                basketProducts = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);
            else
                basketProducts = new List<BasketViewModel>();

            foreach(var basketVM in basketProducts)
            {
                var productDB = _context.Products.FirstOrDefault(x => x.Id == basketVM.ProductId);
                if (productDB == null)
                    continue;

                ProductItemBasketViewModel productItemBasketView = new ProductItemBasketViewModel
                {
                    Product = productDB,
                    Count = basketVM.Count
                };

                basketViewModel.TotalPrice += (int)productDB.Price * basketVM.Count;

                basketViewModel.ProductItemBasketViewModels.Add(productItemBasketView);
            }

            return basketViewModel;
        }
    }
}
