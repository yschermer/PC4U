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
            ShoppingCart shoppingCart = new ShoppingCart();

            if (currentUser != null)
            {
                shoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser).FirstOrDefault();
            }
            else
            {
                shoppingCart.Products = (List<Product>)Session["ShoppingCart"];
            }

            if (shoppingCart == null || shoppingCart.ShoppingCartId == 0)
            {
                //TODO: Vervang dit met een melding dat de winkelwagen leeg is.
                return HttpNotFound();
            }

            return View(shoppingCart.Products);
        }

        // POST: ShoppingCarts/AddToCart
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult AddToCart([Bind(Include = "ProductId")] Product product)
        {
            product = db.Products.Find(product.ProductId);
            string currentUser = HttpContext.User.Identity.GetUserId();

            if (currentUser != null)
            {
                ShoppingCart currentShoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser).FirstOrDefault();

                if (currentShoppingCart != null)
                {
                    // Voeg winkelwagen toe aan huidige.
                    currentShoppingCart.Products.Add(product);
                    db.Entry(currentShoppingCart).State = EntityState.Modified;
                }
                else
                {
                    // Voeg winkelwagen toe.
                    ShoppingCart shoppingCart = new ShoppingCart()
                    {
                        UserId = currentUser,
                        Products = new List<Product>() { product }
                    };
                    db.ShoppingCarts.Add(shoppingCart);
                }
                db.SaveChanges();
            }
            else
            {
                List<Product> products = new List<Product>() { product };

                if (Session["ShoppingCart"] == null)
                {
                    Session["ShoppingCart"] = products;
                }
                else
                {
                    products.AddRange((List<Product>)Session["ShoppingCart"]);
                    Session["ShoppingCart"] = products;
                }
            }
            return RedirectToAction("Index", "Store", null);
        }

        //// GET: ShoppingCarts/EditCart/5
        //public ActionResult EditCart(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
        //    if (shoppingCart == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", shoppingCart.UserId);
        //    return View(shoppingCart);
        //}

        //// POST: ShoppingCarts/EditCart/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditCart([Bind(Include = "ShoppingCartId,UserId")] ShoppingCart shoppingCart)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(shoppingCart).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "FirstName", shoppingCart.UserId);
        //    return View(shoppingCart);
        //}

        //// GET: ShoppingCarts/DeleteFromCart/5
        //public ActionResult DeleteFromCart(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
        //    if (shoppingCart == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(shoppingCart);
        //}

        //// POST: ShoppingCarts/DeleteFromCart/5
        //[HttpPost, ActionName("DeleteFromCart")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteFromCartConfirmed(int id)
        //{
        //    ShoppingCart shoppingCart = db.ShoppingCarts.Find(id);
        //    db.ShoppingCarts.Remove(shoppingCart);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
