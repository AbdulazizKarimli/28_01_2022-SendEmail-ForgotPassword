using FrontToBack.Models;
using FrontToBack.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(x => x.Category).Where(x => !x.IsDeleted).ToList();
            if (products == null)
                return NotFound();

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if(product.Photo == null)
            {
                ModelState.AddModelError("Photo", "image is required");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!product.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "this not image");
                return View();
            }

            string image = FileUtil.GenerateFile(Constants.ImageFolderPath, product.Photo);

            product.Image = image;

            product.CategoryId = 2;

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
