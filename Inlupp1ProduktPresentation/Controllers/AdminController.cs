using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inlupp1ProduktPresentation.Data;
using Inlupp1ProduktPresentation.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;

namespace Inlupp1ProduktPresentation.Controllers
{
    [Authorize(Roles = "Admin, ProductManager")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var viewModel = new AdminIndexViewModel();
            viewModel.NumberOfProducts = _dbContext.Products.Count();
            viewModel.NumberOfCategories = _dbContext.Categories.Count();
            viewModel.NumberOfUsers = _dbContext.Users.Count();
            return View(viewModel);
        }

        public IActionResult AllProducts(string searchInput)
        {
            var viewModel = new AdminAllProductsViewModel();
            viewModel.NumberOfProducts = _dbContext.Products.Count();
            viewModel.Products = _dbContext.Products.Where(dbprod => (searchInput == null) || dbprod.Name.Contains(searchInput)).Select(dbProd => new AdminAllProductsViewModel.AdminProductViewModel()
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
                SelectedCategoryId = dbProd.Category == null ? 0 : dbProd.Category.Id,
                PublishOnWebsite = dbProd.PublishedOnWebsite,
                Categories = GetCategoryListItems()
            };
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
            var productsInCategory = _dbContext.Products.Include(p => p.Category).Where(p => p.Category.Id == id).ToList();
            SetCategoryToNull(productsInCategory);
            _dbContext.Categories.Remove(dbCategory);
            _dbContext.SaveChanges();
            return RedirectToAction("AllCategories");
        }

        private void SetCategoryToNull(List<Product> productsInCategory)
        {
            var list = productsInCategory;
            foreach (var p in list)
            {
                p.Category = null;
            }
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

        [Authorize(Roles = "Admin")]
        public IActionResult AllUsers()
        {
            var viewModel = new AdminAllUsersViewModel();
            viewModel.AllUsers = _dbContext.Users.Select(u => new AdminAllUsersViewModel.RegisteredUser
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToList();
            foreach (var user in viewModel.AllUsers) user.Role = GetRoleName(user.Id);

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult _EditUser(string selectedID)
        {
            var user = _dbContext.Users.First(dbUser => dbUser.Id == selectedID);
            var viewModel = new _EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = GetRoleName(user.Id),
                Roles = GetRolesListItems()
            };
            viewModel.SelectedRoleId = viewModel.Role == null ? "0" : GetRoleId(user.Id);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult _EditUser(string id, _EditUserViewModel model)
        {
            var user = _dbContext.Users.First(u => u.Id == id);
            //var isAllowedToChangeRoleFromAdmin = IsOnlyAdmin(user);

            //if(!isAllowedToChangeRoleFromAdmin)
            //{
            //    ModelState.AddModelError("SelectedRoleId", "Det går ej att ändra från Admin för tillfället. Först måste en annan användare tilldelas denna roll.");
            //}

            //if (model.Password != null)
            //{
            //    var validator = new PasswordValidator<IdentityUser>();
            //    var result = validator.ValidateAsync(_userManager, user, model.Password).Result;
            //    if (!result.Succeeded)
            //    {
            //        ModelState.AddModelError("Password", "Lösenordet måste bestå av minst 6 tecken.");
            //    }

            //}

            if (ModelState.IsValid)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                var selectedRole = _dbContext.Roles.FirstOrDefault(r => r.Id == model.SelectedRoleId);
                if (selectedRole != null) { var r = _userManager.AddToRolesAsync(user, new[] { selectedRole.Name }).Result; }

                //if (model.PreviousSelectedRoleId != "0")
                //{
                //    var previousRole = _dbContext.Roles.First(role => role.Id == model.PreviousSelectedRoleId);
                //    var r = _userManager.RemoveFromRoleAsync(user, previousRole.Name).Result;
                //}
                _dbContext.SaveChanges();
                return RedirectToAction("AllUsers");
            }

            model.Roles = GetRolesListItems();
            return View(model);
        }

        private bool IsOnlyAdmin(IdentityUser user)
        {
            var adminId = _dbContext.Roles.First(r => r.Name == "Admin").Id;
            var adminCount = _dbContext.UserRoles.Count(ur => ur.RoleId == adminId);
            if (adminCount == 1)
            {
                foreach (var u in _dbContext.UserRoles)
                {
                    if (u.UserId == user.Id) return true;
                }
            }
            return false;
        }

        //public IActionResult DeleteUser(string id)
        //{
        //    return View();
        //}
        private string GetRoleId(string userId)
        {
            var userRole = _dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == userId);
            if (userRole == null) return null;
            var roleId = _dbContext.Roles.First(r => r.Id == userRole.RoleId).Id;
            return roleId;
        }
        private string GetRoleName(string userId)
        {
            var userRole = _dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == userId);
            if (userRole == null) return null;
            var roleName = _dbContext.Roles.First(r => r.Id == userRole.RoleId).Name;
            return roleName;
        }

        private List<SelectListItem> GetRolesListItems()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem("Ingen", "0"));
            list.AddRange(_dbContext.Roles.Select(dbRole => new SelectListItem { Text = dbRole.Name, Value = dbRole.Id.ToString() }));
            return list;
        }
    }
}