namespace PC4U.Migrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using Controllers;
    using System.Security.Cryptography;

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
                    UserName = "brian@test.nl",
                    Email = "brian@test.nl",
                    PasswordHash = HashPassword("Horizon123!"),
                    SecurityStamp = "7516ab5e-7279-4b32-9777-388c35dd5e9a",
                    Title = TitleEnum.Man,
                    FirstName = "Brian",
                    LastName = "Lochan",
                    Country = "Nederland",
                    PostalCode = "1111AA",
                    HouseNumber = 1,
                    Street = "Tussen de Bogen",
                    City = "Amsterdam",
                    BirthDate = Convert.ToDateTime("1/1/1990"),
                    TelephoneNumber = "0213456789"
                },
                new ApplicationUser
                {
                    UserName = "larissa@test.nl",
                    Email = "larissa@test.nl",
                    PasswordHash = HashPassword("Horizon123!"),
                    SecurityStamp = "7516ab5e-7279-4b32-9777-388c35dd5e9a",
                    Title = TitleEnum.Vrouw,
                    FirstName = "Larissa",
                    LastName = "Jager",
                    Country = "Nederland",
                    PostalCode = "1111AA",
                    HouseNumber = 1,
                    Street = "Tussen de Bogen",
                    City = "Amsterdam",
                    BirthDate = Convert.ToDateTime("1/1/1990"),
                    TelephoneNumber = "0213456789"
                },
                new ApplicationUser
                {
                    UserName = "yoshio@test.nl",
                    Email = "yoshio@test.nl",
                    PasswordHash = HashPassword("Horizon123!"),
                    SecurityStamp = "7516ab5e-7279-4b32-9777-388c35dd5e9a",
                    Title = TitleEnum.Man,
                    FirstName = "Yoshio",
                    LastName = "Schermer",
                    Country = "Nederland",
                    PostalCode = "1111AA",
                    HouseNumber = 1,
                    Street = "Tussen de Bogen",
                    City = "Amsterdam",
                    BirthDate = Convert.ToDateTime("1/1/1990"),
                    TelephoneNumber = "0213456789"
                }
            };

            List<IdentityRole> roles = new List<IdentityRole>()
            {
                new IdentityRole { Name = "Administrator" }
            };

            users.ForEach(user => context.Users.AddOrUpdate(property => property.UserName, user));
            roles.ForEach(role => context.Roles.AddOrUpdate(property => property.Name, role));
            categories.ForEach(category => context.Categories.AddOrUpdate(property => property.CategoryName, category));
            products.ForEach(product => context.Products.AddOrUpdate(property => property.ProductName, product));

            /* Comment dit uit wanneer je update-database heb uitgevoerd en doe vervolgens update-database opnieuw.
            string[] userIds =
            {
                context.Users.Where(user => user.FirstName == "Brian").FirstOrDefault().Id,
                context.Users.Where(user => user.FirstName == "Larissa").FirstOrDefault().Id,
                context.Users.Where(user => user.FirstName == "Yoshio").FirstOrDefault().Id
            };

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            foreach (string userId in userIds)
            {
                UserManager.AddToRole(userId, context.Roles.FirstOrDefault().Name);
            }
            */
        }

        private string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
    }
}
