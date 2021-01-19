using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlupp1ProduktPresentation.Data;
using Inlupp1ProduktPresentation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Inlupp1ProduktPresentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index(int id)
        {
            var product = _dbContext.Products.First(prod => prod.Id == id);
            var viewModel = new ProductIndexViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
            return View(viewModel);
        }
    }
}
