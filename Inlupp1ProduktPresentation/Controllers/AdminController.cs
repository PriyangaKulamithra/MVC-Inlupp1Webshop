using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inlupp1ProduktPresentation.Data;
using Inlupp1ProduktPresentation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

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
            viewModel.Categories = GetCategoryListItems();
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

            viewModel.Categories = GetCategoryListItems();
            return View(viewModel);
        }

        public IActionResult DeleteProduct(int id)
        {
            var dbProd = _dbContext.Products.First(p => p.Id == id);
            _dbContext.Products.Remove(dbProd);
            _dbContext.SaveChanges();
            return RedirectToAction("AllProducts");
        }

        public IActionResult NewProduct()
        {
            var viewModel = new AdminNewProductViewModel();
            viewModel.Categories = GetCategoryListItems();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult NewProduct(AdminNewProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dbProd = new Product();
                dbProd.Name = model.Name;
                dbProd.Description = model.Description;
                dbProd.Price = model.Price;
                dbProd.PublishedOnWebsite = model.PublishOnWebsite;
                dbProd.Category = _dbContext.Categories.First(c => c.Id == model.SelectedCategoryId);
                _dbContext.Products.Add(dbProd);
                _dbContext.SaveChanges();
                return RedirectToAction("AllProducts");
            }

            model.Categories = GetCategoryListItems();
            return View(model);
        }

        public IActionResult AllCategories()
        {
            var viewModel = new AdminAllCategoriesViewModel();
            viewModel.NumberOfCategories = _dbContext.Categories.Count();
            viewModel.Categories = _dbContext.Categories.Select(c =>
                new AdminAllCategoriesViewModel.AdminCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.CategoryDescription,
                    NumberOfProducts = _dbContext.Products.Count(p => p.Category.Id == c.Id)
                }).ToList();
            return View(viewModel);
        }
        public IActionResult EditCategory(int id)
        {
            var dbCategory = _dbContext.Categories.First(c => c.Id == id);
            var viewModel = new AdminEditCategoryViewModel
            {
                Id = dbCategory.Id,
                Name = dbCategory.Name,
                Description = dbCategory.CategoryDescription,
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult EditCategory(int id, AdminEditCategoryViewModel viewModel)
        {
            var isAlreadyRegisteredName = RegisteredCategoryNames().FirstOrDefault(n => n.ToLower() == viewModel.Name.ToLower());
            if (isAlreadyRegisteredName != null) ModelState.AddModelError("Name", "Detta kategorinamn är redan registrerat.");
            if (ModelState.IsValid)
            {
                var dbCategory = _dbContext.Categories.First(c => c.Id == id);
                dbCategory.Name = viewModel.Name;
                dbCategory.CategoryDescription = viewModel.Description;
                _dbContext.SaveChanges();
                return RedirectToAction("AllCategories");
            }
            return View(viewModel);
        }
        public IActionResult DeleteCategory(int id)
        {
            var dbCategory = _dbContext.Categories.First(c => c.Id == id);
            _dbContext.Categories.Remove(dbCategory);
            _dbContext.SaveChanges();
            return RedirectToAction("AllCategories");
        }

        public IActionResult NewCategory()
        {
            var viewModel = new AdminNewCategoryViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult NewCategory(AdminNewCategoryViewModel viewModel)
        {
            var isAlreadyRegisteredName = RegisteredCategoryNames().FirstOrDefault(n => n.ToLower() == viewModel.Name.ToLower());
            if (isAlreadyRegisteredName != null) ModelState.AddModelError("Name", "Detta kategorinamn är redan registrerat.");
            if (ModelState.IsValid)
            {
                var dbCategory = new ProductCategory();
                dbCategory.Name = viewModel.Name;
                dbCategory.CategoryDescription = viewModel.Description;
                _dbContext.Categories.Add(dbCategory);
                _dbContext.SaveChanges();
                return RedirectToAction("AllCategories");
            }
            return View(viewModel);
        }
        private List<string> RegisteredCategoryNames()
        {
            return _dbContext.Categories.Select(c => c.Name).ToList();
        }

        private List<SelectListItem> GetCategoryListItems()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem("Välj kategori", "0"));
            list.AddRange(_dbContext.Categories.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }));
            return list;
        }
    }
}