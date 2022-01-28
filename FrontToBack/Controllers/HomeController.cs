using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.ViewModels;
using Microsoft.EntityFrameworkCore;
using FrontToBack.Models;
using FrontToBack.Utils;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var categories = _dbContext.Categories.Where(x => x.IsDeleted == false).ToList();
            var products = _dbContext.Products.Include(x => x.Category).Where(x => x.IsDeleted == false && x.Category.IsDeleted == false).ToList();

            var homeViewModel = new HomeViewModel
            {
                Categories = categories,
                Products = products,
            };

            return View(homeViewModel);
        }

        public async Task<IActionResult> Send()
        {
            var message = "<a href='www.google.com'>google</a>";

            await EmailUtil.SendEmailAsync("abdulaziz.karimli@code.edu.az", "test", message);

            return RedirectToAction("Index");
        }
    }
}
