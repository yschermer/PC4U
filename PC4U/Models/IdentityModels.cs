﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PC4U.Models
{
    public enum TitleEnum
    {
        Man,
        Vrouw
    }

    public class ApplicationUser : IdentityUser
    {
        public virtual TitleEnum Title { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Insertion { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Country { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual int HouseNumber { get; set; }
        public virtual string HouseNumberExtension { get; set; }
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual DateTime BirthDate { get; set; }
        public virtual string TelephoneNumber { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartProduct> ShoppingCartProducts { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}