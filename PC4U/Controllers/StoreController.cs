﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PC4U.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PC4U.Controllers
{
    public class StoreController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Store
        public ActionResult Index()
        {
            var products = db.Products.ToList();

            if (products.Count == 0)
            {
                return HttpNotFound();
            }

            ViewBag.Categories = db.Categories.ToList();

            return View(products);
        }

        // GET: Store/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Store/Categories/1
        public ActionResult Categories (int categoryId)
        {
            if (categoryId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Product> products = db.Products.Where(p => p.CategoryId == categoryId).ToList();
            if (products.Count == 0)
            {
                return HttpNotFound();
            }

            ViewBag.Categories = db.Categories.ToList();

            return View("Index", products);
        }

        // GET: Store/Sort?categoryId=1&sort=Asc
        public ActionResult Sort(int? categoryId, string sort)
        {
            string[] sortings = { "Asc", "Desc" };
            List<Product> products = new List<Product>();

            if (categoryId == null || categoryId == 0)
            {
                products = db.Products.ToList();
            }
            else
            {
                products = db.Products.Where(p => p.CategoryId == categoryId).ToList();
            }

            if (products.Count == 0)
            {
                return HttpNotFound();
            }

            if (sort != sortings[0])
            {
                if (sort == sortings[1])
                {
                    products = products.OrderByDescending(p => p.Price).ToList();
                }
            }
            else
            {
                products = products.OrderBy(p => p.Price).ToList();
            }

            ViewBag.Categories = db.Categories.ToList();

            return View("Index", products);
        }
    }
}
