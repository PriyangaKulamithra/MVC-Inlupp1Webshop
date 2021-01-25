using System.Collections.Generic;
using System.Linq;
using Inlupp1ProduktPresentation.Data;
using Inlupp1ProduktPresentation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Inlupp1ProduktPresentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public AdminController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var viewModel = new AdminIndexViewModel();
            viewModel.NumberOfProducts = _dbContext.Products.Count();
            viewModel.NumberOfCategories = _dbContext.Categories.Count();
            return View(viewModel);
        }

        public IActionResult AllProducts()
        {
            var viewModel = new AdminAllProductsViewModel();
            viewModel.NumberOfProducts = _dbContext.Products.Count();
            viewModel.Products = _dbContext.Products.Select(dbProd => new AdminAllProductsViewModel.AdminProductViewModel()
            {
                Id = dbProd.Id,
                Name = dbProd.Name,
                Description = dbProd.Description,
                Category = dbProd.Category.Name,
                Price = dbProd.Price,
                IsPublished = dbProd.PublishedOnWebsite
            }).ToList();
            return View(viewModel);
        }

        public IActionResult EditProduct(int id)
        {
            var dbProd = _dbContext.Products.Include(p => p.Category).First(prod => prod.Id == id);
            var viewModel = new AdminEditProductViewModel
            {
                Id = dbProd.Id,
                Name = dbProd.Name,
                Description = dbProd.Description,
                Price = dbProd.Price,
                SelectedCategoryId = dbProd.Category.Id,
                PublishOnWebsite = dbProd.PublishedOnWebsite
            };
            viewModel.Categories = GetAllCategories();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult EditProduct(int id, AdminEditProductViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var dbProd = _dbContext.Products.Include(p => p.Category).First(pr => pr.Id == id);
                dbProd.Name = viewModel.Name;
                dbProd.Description = viewModel.Description;
                dbProd.Price = viewModel.Price;
                dbProd.Category = _dbContext.Categories.First(c => c.Id == viewModel.SelectedCategoryId);
                dbProd.PublishedOnWebsite = viewModel.PublishOnWebsite;
                _dbContext.SaveChanges();
                return RedirectToAction("AllProducts");
            }

            viewModel.Categories = GetAllCategories();
            return View(viewModel);
        }

        public IActionResult DeleteProduct(int id)
        {
            var dbProd = _dbContext.Products.First(p => p.Id == id);
            _dbContext.Products.Remove(dbProd);
            _dbContext.SaveChanges();
            return RedirectToAction("AllProducts");
        }
        public IActionResult EditCategories()
        {
            var viewModel = new AdminAllCategoriesViewModel();
            return View(viewModel);
        }

        private List<SelectListItem> GetAllCategories()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem("Välj kategori", "0"));
            list.AddRange(_dbContext.Categories.Select(c=>new SelectListItem{Text = c.Name, Value = c.Id.ToString()}));
            return list;
        }
    }
}