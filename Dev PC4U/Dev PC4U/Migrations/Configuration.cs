namespace Dev_PC4U.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Dev_PC4U.Models;
    using System.Collections.Generic;

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

            List<Products> products = new List<Products>()
            {
                new Products { ProductName = "Lenovo Z50-70"},
                new Products { ProductName = "Asus X75V"},
                new Products { ProductName = "Dell HorizonCollege"}
            };

            context.Products.Add(products[0]);
            context.Products.Add(products[1]);
            context.Products.Add(products[2]);
        }
    }
}
