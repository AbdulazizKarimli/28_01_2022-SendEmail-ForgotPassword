using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FrontToBack.Models;
using Newtonsoft.Json;
using FrontToBack.ViewModels;

namespace FrontToBack.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly int _productsCount;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _productsCount = _dbContext.Products.Count();
        }

        public IActionResult Index()
        {
            ViewBag.Products = _productsCount;

            var products = _dbContext.Products.Include(x => x.Category)
                .Where(x => x.IsDeleted == false && x.Category.IsDeleted == false).OrderByDescending(x => x.Id).Take(8).ToList();
            return View(products);
        }

        #region LoadMore

        public IActionResult Load(int skip)
        {
            if (skip >= _productsCount)
                return BadRequest();

            var products = _dbContext.Products.Include(x => x.Category).OrderByDescending(x => x.Id).Skip(skip).Take(8).ToList();

            return PartialView("_ProductPartial", products);
        }

        #endregion

        #region AddToBasket

        public IActionResult AddToBasket(int? id)
        {
            if (id == null)
                return BadRequest();

            var dbProducts = _dbContext.Products.FirstOrDefault(x => x.Id == id);
            if (dbProducts == null)
                return NotFound();

            string basket = HttpContext.Request.Cookies["basket"];
            List<BasketViewModel> basketVms;

            if (string.IsNullOrEmpty(basket))
                basketVms = new List<BasketViewModel>();
            else
                basketVms = JsonConvert.DeserializeObject<List<BasketViewModel>>(basket);

            var existsProduct = basketVms.FirstOrDefault(x => x.ProductId == id.Value);
            if(existsProduct == null)
            {
                BasketViewModel basketViewModel = new BasketViewModel
                {
                    ProductId = id.Value,
                    Count = 1
                };
                basketVms.Add(basketViewModel);
            }
            else
            {
                existsProduct.Count++;
            }

            var newBasket = JsonConvert.SerializeObject(basketVms);
            HttpContext.Response.Cookies.Append("basket", newBasket);

            return PartialView("_BasketPartialView", _getBasket(basketVms));
        }

        private ProductBasketViewModel _getBasket(List<BasketViewModel> basketViewModels)
        {
            ProductBasketViewModel basketViewModel = new ProductBasketViewModel
            {
                ProductItemBasketViewModels = new List<ProductItemBasketViewModel>(),
                TotalPrice = 0
            };

            foreach (var basket in basketViewModels)
            {
                var productDB = _dbContext.Products.FirstOrDefault(x => x.Id == basket.ProductId);
                if (productDB == null)
                    continue;

                ProductItemBasketViewModel productItemBasketView = new ProductItemBasketViewModel
                {
                    Product = productDB,
                    Count = basket.Count
                };

                basketViewModel.TotalPrice += (int)productDB.Price * basket.Count;

                basketViewModel.ProductItemBasketViewModels.Add(productItemBasketView);
            }

            return basketViewModel;
        }

        #endregion
    }
}
