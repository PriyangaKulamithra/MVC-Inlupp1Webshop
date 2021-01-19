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

        public IActionResult Index()
        {
            var viewModel = new ProductIndexViewModel
            {
                Title = "Alla Produkter",
                Products = _dbContext.Products.Select(dbProd => new ProductViewModel
                {
                    Id = dbProd.Id,
                    Description = dbProd.Description,
                    Name = dbProd.Name,
                    Price = dbProd.Price
                }).ToList()
            };
            return View(viewModel);
        }
        public IActionResult Details(int id)
        {
            var product = _dbContext.Products.First(prod => prod.Id == id);
            var viewModel = new ProductDetailsViewModel()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
            return View(viewModel);
        }
    }
}
