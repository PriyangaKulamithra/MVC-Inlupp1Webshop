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
            //SeedUsers(dbContext); //BEHÖVS DENNA? 
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
                    Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter"),
                    PublishedOnWebsite = true
                });

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Elefantöra stor");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Elefantöra stor",
                    Description = "Höjd 25cm",
                    Price = 79.9f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter"),
                    PublishedOnWebsite = true
                });

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Monstera");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Monstera",
                    Description = "Höjd 55cm",
                    Price = 159f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter"),
                    PublishedOnWebsite = true
                });

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Moses Stentavla");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Moses Stentavla",
                    Description = "Höjd 30cm",
                    Price = 149f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Gröna växter"),
                    PublishedOnWebsite = true
                });

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Kruka - STOR");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Kruka - STOR",
                    Description = "Kruka med dräneringshål i botten. 17cm i diameter.",
                    Price = 119f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Krukor och tillbehör"),
                    PublishedOnWebsite = true
                });

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Kruka - MELLAN");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Kruka - MELLAN",
                    Description = "Kruka med dräneringshål i botten. 13cm i diameter.",
                    Price = 79f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Krukor och tillbehör"),
                    PublishedOnWebsite = true
                });

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Kruka - LITEN");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Kruka - LITEN",
                    Description = "Terrakottakruka med patina. 7cm i diameter.",
                    Price = 29f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Krukor och tillbehör"),
                    PublishedOnWebsite = true
                });

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Tulpaner 10-pack");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Tulpaner 10-pack",
                    Description = "Liten tulpanbukett i flera färger.",
                    Price = 59.9f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Snittblommor"),
                    PublishedOnWebsite = true
                });

            product = dbContext.Products.FirstOrDefault(prod => prod.Name == "Tulpaner 30-pack");
            if (product == null)
                dbContext.Products.Add(new Product()
                {
                    Name = "Tulpaner 30-pack",
                    Description = "Stor tulpanbukett i flera färger.",
                    Price = 199.9f,
                    Category = dbContext.Categories.First(cat => cat.Name == "Snittblommor"),
                    PublishedOnWebsite = true
                });

            dbContext.SaveChanges();
        }

        private static void SeedCategories(ApplicationDbContext dbContext)
        {
            var category = dbContext.Categories.FirstOrDefault(cat => cat.Name == "Krukor och tillbehör");
            if (category == null)
                dbContext.Categories.Add(new ProductCategory()
                {
                    Name = "Krukor och tillbehör",
                    CategoryDescription = "Alla våra växter behöver en vän som håller om dem och som förhöjer deras stil, så här hittar du alla våra tillbehör till våra växter."
                });
            else
            {
                category.CategoryDescription =
                    "Alla våra växter behöver en vän som håller om dem och som förhöjer deras stil, så här hittar du alla våra tillbehör till våra växter.";
            }

            category = dbContext.Categories.FirstOrDefault(cat => cat.Name == "Snittblommor");
            if (category == null)
                dbContext.Categories.Add(new ProductCategory()
                {
                    Name = "Snittblommor",
                    CategoryDescription = "Fira in att det är måndag/tisdag/vilkendagsomhelst med en ljuvlig bukett blommor"
                });
            else
            {
                category.CategoryDescription =
                    "Fira in att det är måndag/ tisdag / vilkendagsomhelst med en ljuvlig bukett blommor";
            }

            category = dbContext.Categories.FirstOrDefault(cat => cat.Name == "Gröna växter");
            if (category == null)
                dbContext.Categories.Add(new ProductCategory()
                {
                    Name = "Gröna Växter",
                    CategoryDescription = "Vet du inte riktigt vad du letar efter men känner att du har alldeles för lite grönt i ditt liv eller bara känner att en " +
                                          "växt skulle va det bästa som hänt dig? Då har du kommit rätt. "
                });
            else
            {
                category.CategoryDescription =
                    "Vet du inte riktigt vad du letar efter men känner att du har alldeles för lite grönt i ditt liv eller bara känner att en " +
                    "växt skulle va det bästa som hänt dig? Då har du kommit rätt.";
            }

            dbContext.SaveChanges();
        }

        //private static void SeedUsers(ApplicationDbContext dbContext)
        //{

        //}

    }
}
