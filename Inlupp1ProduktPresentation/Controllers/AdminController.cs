using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inlupp1ProduktPresentation.Data;
using Inlupp1ProduktPresentation.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


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
            if (viewModel.Name != null)
            {
                var isAlreadyRegisteredName = RegisteredCategoryNames().FirstOrDefault(n => n.ToLower() == viewModel.Name.ToLower());
                if (isAlreadyRegisteredName != null) ModelState.AddModelError("Name", "Detta kategorinamn är redan registrerat.");
            }

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

            viewModel.AllUsers = _userManager.Users.Select(u => new AdminAllUsersViewModel.RegisteredUser
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToList();
            foreach (var user in viewModel.AllUsers) user.Role = GetRoleName(user.Id);

            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult NewUser()
        {
            var viewModel = new AdminNewUserViewModel();
            viewModel.Roles = GetRolesListItems();
            return View(viewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult NewUser(AdminNewUserViewModel model)
        {
            if (_dbContext.Users.Any(u=>u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Denna mailadress är redan registrerad.");
            }
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    EmailConfirmed = true,
                };
                _dbContext.AddAsync(user);
                var roles = GetRoleName(model.SelectedRoleId);
                if (roles != null)
                {
                    var r = _userManager.AddToRolesAsync(user, new[] { roles }).Result;
                }
                
                _dbContext.SaveChanges();
                return RedirectToAction("AllUsers");
            }
            model.Roles = GetRolesListItems();
            return View(model);
        }
  

        [Authorize(Roles = "Admin")]
        public IActionResult _EditUser(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var viewModel = new _EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };
            viewModel.SelectedRoleId = GetRoleId(user.Id);
            viewModel.Roles = GetRolesListItems();
            return View(viewModel);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult _EditUser(string id, _EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _dbContext.Users.First(u => u.Id == id);
                user.Email = model.Email;
                user.UserName = model.UserName;
                if (model.SelectedRoleId != null && model.SelectedRoleId != "0")
                {
                    var selectedRole = _dbContext.Roles.First(r => r.Id == model.SelectedRoleId);
                    var isInRole = _userManager.IsInRoleAsync(user, selectedRole.Name).Result;
                    if (!isInRole)
                    {
                        var userRole = _dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == id);
                        if (userRole != null)
                        {
                            var currentRoleName = GetRoleName(user.Id);
                            var r = _userManager.RemoveFromRoleAsync(user, currentRoleName).Result;
                            var res = _userManager.AddToRoleAsync(user, selectedRole.Name).Result;
                        }
                        else
                        {
                            var r = _userManager.AddToRoleAsync(user, selectedRole.Name).Result;
                        }
                    }
                }
                else
                {
                    var userRole = _dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == model.Id);
                    if (userRole != null)
                    {
                        var role = _dbContext.Roles.First(r => r.Id == userRole.RoleId);
                        var r =_userManager.RemoveFromRoleAsync(user, role.Name).Result;
                    }
                }
                _dbContext.SaveChanges();
            }
            return RedirectToAction("AllUsers");
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user != null)
            {
                var admins = _userManager.GetUsersInRoleAsync("Admin").Result;
                var isAdmin = _userManager.IsInRoleAsync(user, "Admin").Result;
                if (isAdmin && admins.Count == 1)
                {
                    return RedirectToAction("AllUsers");
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded) return RedirectToAction("AllUsers");
            }

            return RedirectToAction("AllUsers");
        }
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