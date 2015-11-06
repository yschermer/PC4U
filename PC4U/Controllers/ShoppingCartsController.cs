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
                shoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser && s.Status == StatusEnum.Unordered).FirstOrDefault();

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

            if (shoppingCartProducts == null)
            {
                //TODO: Vervang dit met een melding dat de winkelwagen leeg is.
                return RedirectToAction("Index", "Store", null);
            }

            return View(shoppingCartProducts);
        }

        // POST: ShoppingCarts/AddToCart
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult AddToCart([Bind(Include = "ShoppingCartId, ProductId, AmountOfProducts")] ShoppingCartProduct shoppingCartProduct)
        {
            string currentUser = HttpContext.User.Identity.GetUserId();

            if (currentUser != null)
            {
                ShoppingCart currentShoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser).FirstOrDefault();

                if (currentShoppingCart != null)
                {
                    // Voeg winkelwagen toe aan bestaande winkelwagen.
                    ShoppingCartProduct currentShoppingCartProduct = new ShoppingCartProduct()
                    {
                        ShoppingCartId = currentShoppingCart.ShoppingCartId,
                        ProductId = shoppingCartProduct.ProductId,
                        AmountOfProducts = shoppingCartProduct.AmountOfProducts
                    };

                    if (db.ShoppingCartProducts.Where(s => s.ProductId == currentShoppingCartProduct.ProductId && s.ShoppingCartId == currentShoppingCart.ShoppingCartId).Any())
                    {
                        db.Entry(currentShoppingCartProduct).State = EntityState.Modified;
                    }
                    else
                    {
                        db.ShoppingCartProducts.Add(currentShoppingCartProduct);
                    }
                }
                else
                {
                    // Maak een nieuwe winkelwagen aan.
                    ShoppingCart shoppingCart = new ShoppingCart()
                    {
                        UserId = currentUser,
                        Products = new List<Product>()
                        {
                            db.Products.Find(shoppingCartProduct.ProductId)
                        }
                    };
                    db.ShoppingCarts.Add(shoppingCart);
                    db.SaveChanges();

                    currentShoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser).FirstOrDefault();

                    ShoppingCartProduct currentShoppingCartProduct = new ShoppingCartProduct()
                    {
                        ShoppingCartId = currentShoppingCart.ShoppingCartId,
                        ProductId = shoppingCartProduct.ProductId,
                        AmountOfProducts = shoppingCartProduct.AmountOfProducts
                    };
                    db.Entry(currentShoppingCartProduct).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
            else
            {
                List<ShoppingCartProduct> shoppingCartProducts = new List<ShoppingCartProduct>();
                var temp = (List<ShoppingCartProduct>)Session["ShoppingCart"];
                if (Session["ShoppingCart"] == null || temp.Count == 0)
                {
                    shoppingCartProducts.Add(shoppingCartProduct);
                    Session["ShoppingCart"] = shoppingCartProducts;
                }
                else
                {
                    shoppingCartProducts.AddRange((List<ShoppingCartProduct>)Session["ShoppingCart"]);
                    if (!shoppingCartProducts.Where(s => s.ProductId == shoppingCartProduct.ProductId).Any())
                    {
                        shoppingCartProducts.Add(shoppingCartProduct);
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
