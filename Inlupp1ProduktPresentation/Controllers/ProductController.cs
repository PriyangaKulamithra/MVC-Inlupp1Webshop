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

        public IActionResult Index(string searchInput, string sortOrder)
        {
            var viewModel = new ProductIndexViewModel
            {
                Products = _dbContext.Products
                    .Where(dbP => (searchInput == null || dbP.Name.Contains(searchInput) || dbP.Description.Contains(searchInput)) && dbP.PublishedOnWebsite)
                    .Select(dbProd => new ProductIndexViewModel.ProductViewModel
                    {
                        Id = dbProd.Id,
                        Description = dbProd.Description,
                        Name = dbProd.Name,
                        Price = dbProd.Price,
                        ImageName = ConvertToImageName(dbProd.Name)
                    }).ToList()
            };

            viewModel.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            viewModel.PriceSort = sortOrder == "price_asc" ? "price_desc" : "price_asc";

            switch (viewModel.NameSort)
            {
                case "name_asc":
                    viewModel.Products = viewModel.Products.OrderBy(p => p.Name).ToList();
                    break;
                case "price_asc":
                    viewModel.Products = viewModel.Products.OrderBy(p => p.Price).ToList();
                    break;
                case "price_desc":
                    viewModel.Products = viewModel.Products.OrderByDescending(p => p.Price).ToList();
                    break;
                default:
                    viewModel.Products = viewModel.Products.OrderByDescending(p => p.Name).ToList();
                    break;
            }
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

        public IActionResult Details(int id)
        {
            var product = _dbContext.Products.First(prod => prod.Id == id);
            var viewModel = new ProductDetailsViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageName = ConvertToImageName(product.Name)
            };
            return View(viewModel);
        }
    }
}
