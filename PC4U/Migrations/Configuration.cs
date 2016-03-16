namespace PC4U.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            List<Category> categories = new List<Category>()
            {
                new Category { CategoryName = "Laptop" },
                new Category { CategoryName = "Desktop" }
            };

            List<Product> products = new List<Product>()
            {
                new Product { ProductName = "Lenovo Z50-70", Price = 499.99M, CategoryId = 1},
                new Product { ProductName = "Asus X75V", Price = 599.99M, CategoryId = 1},
                new Product { ProductName = "Dell HorizonCollege", Price = 9.99M, CategoryId = 1}
            };

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    UserName = "Tester",
                    Email = "admin@admin.nl",
                    SecurityStamp = "7516ab5e-7279-4b32-9777-388c35dd5e9a",
                    Title = TitleEnum.MALE,
                    FirstName = "Admin",
                    LastName = "Admin",
                    Country = "Nederland",
                    PostalCode = "1111AA",
                    HouseNumber = 1,
                    Street = "Tussen de Bogen",
                    City = "Amsterdam",
                    BirthDate = Convert.ToDateTime("1/1/1990"),
                    TelephoneNumber = "0123456789"
                }
            };

            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole { Name = "Administrator" }
            };

            roles.ForEach(role => context.Roles.AddOrUpdate(property => property.Name, role));
            categories.ForEach(category => context.Categories.AddOrUpdate(property => property.CategoryName, category));
            products.ForEach(product => context.Products.AddOrUpdate(property => property.ProductName, product));

            // Creates users and adds roles automatically
            foreach (ApplicationUser user in users)
            {
                Helpers.Helper.CreateUserByRole(Helpers.Helper.RoleEnum.Administrator, user, "Horizon123!");
            }
        }
    }
}
