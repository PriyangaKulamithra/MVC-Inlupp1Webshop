using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Inlupp1ProduktPresentation.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Inlupp1ProduktPresentation.Models;
using Inlupp1ProduktPresentation.Models.ViewModels;

namespace Inlupp1ProduktPresentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeIndexViewModel();
            viewModel.Categories = _dbContext.Categories.Select(dbCategory => new HomeIndexViewModel.CategoryViewModel
            {
                Name = dbCategory.Name,
                Id = dbCategory.Id,
                Description = dbCategory.CategoryDescription,
                ImageName = ConvertToImageName(dbCategory.Name)
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
