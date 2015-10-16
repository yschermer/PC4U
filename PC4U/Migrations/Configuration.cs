namespace PC4U.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            List<Category> categories = new List<Category>()
            {
                new Category { CategoryName = "Laptop" }
            };

            categories.ForEach(category => context.Categories.AddOrUpdate(property => property.CategoryName, category));

            List<Product> products = new List<Product>()
            {
                new Product { ProductName = "Lenovo Z50-70", Price = 499.99M, CategoryId = 1},
                new Product { ProductName = "Asus X75V", Price = 599.99M, CategoryId = 1},
                new Product { ProductName = "Dell HorizonCollege", Price = 9.99M, CategoryId = 1}
            };

            products.ForEach(product => context.Products.AddOrUpdate(property => property.ProductName, product));
        }
    }
}
