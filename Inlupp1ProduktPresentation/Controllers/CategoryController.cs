using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlupp1ProduktPresentation.Data;
using Inlupp1ProduktPresentation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Inlupp1ProduktPresentation.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        
        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var viewModel = new CategoryIndexViewModel();
            viewModel.Categories = _dbContext.Categories.Select(dbCat => new CategoryViewModel
            {
                Id = dbCat.Id,
                Name = dbCat.Name,
                ImageName = ConvertToImageName(dbCat.Name)
            }).ToList();
            return View(viewModel);
        }
        public IActionResult GetProducts(int id)
        {
            var model = _dbContext.Categories.First(cat => cat.Id == id);
            var viewModel = new CategoryGetProductsViewModel{ CategoryName = model.Name, CategoryDescription = model.CategoryDescription};
            
            var products = _dbContext.Products.Where(prod => prod.Category.Id == id && prod.PublishedOnWebsite).ToList();
            viewModel.Products = products.Select(prod => new ProductViewModel()
            {
                Id = prod.Id,
                Name = prod.Name,
                Description = prod.Description,
                Price = prod.Price,
                ImageName = ConvertToImageName(prod.Name)
            }).ToList();

            return View(viewModel);
        }
        private static string ConvertToImageName(string name)
        {
            var imgName = name.ToLower();
            imgName = imgName.Replace(' ', '_');
            imgName = imgName.Replace('å', 'a');
            imgName = imgName.Replace('ä', 'a');
            imgName = imgName.Replace('ö', 'o');
            return imgName;
        }
    }
}
