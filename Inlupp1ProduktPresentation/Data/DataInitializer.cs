using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Inlupp1ProduktPresentation.Data
{
    public class DataInitializer
    {
        public static void SeedData(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            dbContext.Database.Migrate();
            SeedProducts(dbContext);
            SeedCategories(dbContext);
            SeedUsers(dbContext); //BEHÖVS DENNA? 
        }

        private static void SeedProducts(ApplicationDbContext dbContext)
        {
            var product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Elefantöra liten");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Elefantöra liten",
                    Description = "Höjd 8cm",
                    Price = 29.9f,
                    Category = dbContext.Categories.First(cat=>cat.Name == "Gröna växter")
                });
            else
            {
                product.Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter");
            }

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Elefantöra stor");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Elefantöra stor",
                    Description = "Höjd 25cm",
                    Price = 79.9f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter")
                });
            else
            {
                product.Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter");
            }

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Monstera");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Monstera",
                    Description = "Höjd 55cm",
                    Price = 159f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter")
                });
            else
            {
                product.Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter");
            }

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Moses Stentavla");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Moses Stentavla",
                    Description = "Höjd 30cm",
                    Price = 149f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter")
                });
            else
            {
                product.Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter");
            }

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Kruka - STOR");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Kruka - STOR",
                    Description = "Kruka med dräneringshål i botten. 17cm i diameter.",
                    Price = 119f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Krukor")
                });
            else
            {
                product.Category = dbContext.Categories.First(cat => cat.Name == "Krukor");
            }

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Kruka - MELLAN");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Kruka - MELLAN",
                    Description = "Kruka med dräneringshål i botten. 13cm i diameter.",
                    Price = 79f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Krukor")
                });
            else
            {
                product.Category = dbContext.Categories.First(cat => cat.Name == "Krukor");
            }

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Kruka - LITEN");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Kruka - LITEN",
                    Description = "Terrakottakruka med patina. 7cm i diameter.",
                    Price = 29f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Krukor")
                });
            else
            {
                product.Category = dbContext.Categories.First(cat => cat.Name == "Krukor");
            }

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Tulpaner 10-pack");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Tulpaner 10-pack",
                    Description = "Liten tulpanbukett i flera färger.",
                    Price = 59.9f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Snittblommor")
                });
            else
            {
                product.Category = dbContext.Categories.First(cat => cat.Name == "Snittblommor");
            }

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Tulpaner 30-pack");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Tulpaner 30-pack",
                    Description = "Stor tulpanbukett i flera färger.",
                    Price = 199.9f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Snittblommor")
                });
            else
            {
                product.Category = dbContext.Categories.First(cat => cat.Name == "Snittblommor");
            }

            dbContext.SaveChanges();
        }

        private static void SeedCategories(ApplicationDbContext dbContext)
        {
            var category = dbContext.Categories.FirstOrDefault(cat => cat.Name == "Krukor");
            if (category == null)
                dbContext.Categories.Add(new ProductCategory()
                {
                    Name = "Krukor"
                });

            category = dbContext.Categories.FirstOrDefault(cat => cat.Name == "Snittblommor");
            if (category == null)
                dbContext.Categories.Add(new ProductCategory()
                {
                    Name = "Snittblommor"
                });

            category = dbContext.Categories.FirstOrDefault(cat => cat.Name == "Gröna växter");
            if (category == null)
                dbContext.Categories.Add(new ProductCategory()
                {
                    Name = "Gröna Växter"
                });

            dbContext.SaveChanges();
        }

        private static void SeedUsers(ApplicationDbContext dbContext)
        {
            
        }
        
    }
}
