using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PC4U.Models;
using Microsoft.AspNet.Identity;

namespace PC4U.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShoppingCarts
        public ActionResult Index()
        {
            string currentUser = HttpContext.User.Identity.GetUserId();
            ShoppingCart shoppingCart = null;
            List<ShoppingCartProduct> shoppingCartProducts = new List<ShoppingCartProduct>();

            if (currentUser != null)
            {
                shoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser).FirstOrDefault();

                if(shoppingCart != null)
                {
                    shoppingCartProducts = db.ShoppingCartProducts.Where(s => s.ShoppingCartId == shoppingCart.ShoppingCartId).OrderBy(p => p.ProductId).ToList();
                }
            }
            else
            {
                shoppingCartProducts = (List<ShoppingCartProduct>)Session["ShoppingCart"];
                if(shoppingCartProducts != null)
                {
                    foreach (ShoppingCartProduct s in shoppingCartProducts)
                    {
                        s.Product = db.Products.Where(p => p.ProductId == s.ProductId).FirstOrDefault();
                    }
                }
            }

            if (shoppingCartProducts == null || shoppingCart == null)
            {
                //TODO: Vervang dit met een melding dat de winkelwagen leeg is.
                return RedirectToAction("Index", "Store", null);
            }
            else
            {
                decimal priceVat = 0.00M;
                foreach(ShoppingCartProduct shoppingCartProduct in shoppingCartProducts)
                {
                    decimal temp = db.Products.Find(shoppingCartProduct.ProductId).Price * shoppingCartProduct.AmountOfProducts;
                    priceVat += temp;
                }
                ViewBag.PriceVat = string.Format("{0:C}", priceVat);
                ViewBag.PriceNonVat = string.Format("{0:C}", (priceVat * 81)/100);
            }

            return View(shoppingCartProducts);
        }

        // POST: ShoppingCarts/AddToCart
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult AddToCart([Bind(Include = "ShoppingCartId, ProductId, AmountOfProducts")] ShoppingCartProduct input)
        {
            string currentUser = HttpContext.User.Identity.GetUserId();

            // Als de gebruiker is ingelogd.
            if (currentUser != null)
            {
                // Bestaande winkelwagen ophalen.
                ShoppingCart existingShoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser).FirstOrDefault();

                // Als de gebruiker al een winkelwagen bezit in de database.
                if (existingShoppingCart != null)
                {
                    ShoppingCartProduct shoppingCartProduct = new ShoppingCartProduct()
                    {
                        ShoppingCartId = existingShoppingCart.ShoppingCartId,
                        ProductId = input.ProductId,
                        AmountOfProducts = input.AmountOfProducts
                    };

                    if (db.ShoppingCartProducts.Where(s => s.ProductId == shoppingCartProduct.ProductId && s.ShoppingCartId == existingShoppingCart.ShoppingCartId).Any())
                    {
                        db.Entry(shoppingCartProduct).State = EntityState.Modified;
                    }
                    else
                    {
                        db.ShoppingCartProducts.Add(shoppingCartProduct);
                    }
                }
                // Als de gebruiker nog geen winkelwagen bezit.
                else
                {
                    ShoppingCart shoppingCart = new ShoppingCart()
                    {
                        ShoppingCartId = db.ShoppingCarts.Any() ? db.ShoppingCarts.Max(s => s.ShoppingCartId) : 1,
                        UserId = currentUser
                    };

                    ShoppingCartProduct shoppingCartProduct = new ShoppingCartProduct()
                    {
                        ShoppingCart = shoppingCart,
                        ShoppingCartId = shoppingCart.ShoppingCartId,
                        ProductId = input.ProductId,
                        AmountOfProducts = input.AmountOfProducts
                    };
                    db.ShoppingCarts.Add(shoppingCart);
                    db.ShoppingCartProducts.Add(shoppingCartProduct);
                }
                db.SaveChanges();
            }
            // Als de gebruiker niet is ingelogt.
            else
            {
                List<ShoppingCartProduct> shoppingCartProducts = new List<ShoppingCartProduct>();
                var temp = (List<ShoppingCartProduct>)Session["ShoppingCart"];
                if (Session["ShoppingCart"] == null || temp.Count == 0)
                {
                    shoppingCartProducts.Add(input);
                    Session["ShoppingCart"] = shoppingCartProducts;
                }
                else
                {
                    shoppingCartProducts.AddRange((List<ShoppingCartProduct>)Session["ShoppingCart"]);
                    if (!shoppingCartProducts.Where(s => s.ProductId == input.ProductId).Any())
                    {
                        shoppingCartProducts.Add(input);
                    }
                    Session["ShoppingCart"] = shoppingCartProducts;
                }
            }
            return RedirectToAction("Index", "Store", null);
        }

        // POST: ShoppingCarts/EditCart/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCart([Bind(Include = "ShoppingCartId, ProductId, AmountOfProducts")] ShoppingCartProduct shoppingCartProduct)
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var currentUser = db.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {
                    ShoppingCart shoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser.Id).FirstOrDefault();
                    shoppingCartProduct.ShoppingCartId = shoppingCart.ShoppingCartId;
                    db.Entry(shoppingCartProduct).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    List<ShoppingCartProduct> shoppingCartProducts = (List<ShoppingCartProduct>)Session["ShoppingCart"];
                    ShoppingCartProduct tempShoppingCartProduct = shoppingCartProducts.Where(s => s.ProductId == shoppingCartProduct.ProductId && s.ShoppingCartId == shoppingCartProduct.ShoppingCartId).FirstOrDefault();
                    shoppingCartProducts.Remove(tempShoppingCartProduct);
                    shoppingCartProducts.Add(shoppingCartProduct);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: ShoppingCarts/DeleteFromCart/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFromCart([Bind(Include = "ShoppingCartId, ProductId")] ShoppingCartProduct shoppingCartProduct)
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var currentUser = db.Users.Where(u => u.Id == userId).FirstOrDefault();

            if(currentUser != null)
            {
                ShoppingCart shoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser.Id).FirstOrDefault();
                shoppingCartProduct = db.ShoppingCartProducts.Where(s => s.ShoppingCartId == shoppingCart.ShoppingCartId && s.ProductId == shoppingCartProduct.ProductId).FirstOrDefault();
                db.ShoppingCartProducts.Remove(shoppingCartProduct);
                db.SaveChanges();
            }
            else
            {
                List<ShoppingCartProduct> shoppingCartProducts = (List<ShoppingCartProduct>)Session["ShoppingCart"];
                ShoppingCartProduct tempShoppingCartProduct = shoppingCartProducts.Where(s => s.ProductId == shoppingCartProduct.ProductId && s.ShoppingCartId == shoppingCartProduct.ShoppingCartId).FirstOrDefault();
                shoppingCartProducts.Remove(tempShoppingCartProduct);
            }

            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
