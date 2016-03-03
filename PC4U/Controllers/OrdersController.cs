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
using System.IO;
using System.Net.Mail;

namespace PC4U.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Orders
        //public ActionResult Index()
        //{
        //    return View(db.Orders.ToList());
        //}

        //// GET: Orders/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// GET: Orders/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceMail")] bool invoiceMail)
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var currentUser = db.Users.Where(u => u.Id == userId).FirstOrDefault();

            Order order = new Order();
            order.CustomerId = currentUser.Id;
            order.ShoppingCartId = db.ShoppingCarts.Include(s => s.User).Where(s => s.UserId == currentUser.Id).Select(s => s.ShoppingCartId).FirstOrDefault();

            ShoppingCart shoppingCart = db.ShoppingCarts.Include(s => s.User).Where(s => s.UserId == currentUser.Id).FirstOrDefault();

            OldShoppingCart oldShoppingCart = new OldShoppingCart()
            {
                ShoppingCartId = shoppingCart.ShoppingCartId,
                UserId = shoppingCart.UserId
            };

            List<ShoppingCartProduct> shoppingCartProducts = db.ShoppingCartProducts.Include(s => s.Product).Where(s => s.ShoppingCartId == shoppingCart.ShoppingCartId).ToList();
            decimal priceVat = 0.00M;
            foreach (ShoppingCartProduct shoppingCartProduct in shoppingCartProducts)
            {
                decimal temp = db.Products.Find(shoppingCartProduct.ProductId).Price * shoppingCartProduct.AmountOfProducts;
                priceVat += temp;
            }
            ViewBag.PriceVat = string.Format("{0:C}", priceVat);
            ViewBag.Vat = string.Format("{0:C}", (priceVat * 21) / 100);
            ViewBag.PriceNonVat = string.Format("{0:C}", (priceVat * 79) / 100);

            db.Orders.Add(order);
            db.OldShoppingCarts.Add(oldShoppingCart);
            db.ShoppingCarts.Remove(shoppingCart);
            db.SaveChanges();

            if (invoiceMail)
            {
                string relativePath = "~/Views/Orders/CheckoutMail.cshtml";
                var content = string.Empty;
                var view = ViewEngines.Engines.FindView(ControllerContext, relativePath, null);
                foreach(ShoppingCartProduct s in shoppingCartProducts)
                {
                    s.Product = db.Products.Find(s.ProductId);
                }
                ViewData.Model = shoppingCartProducts;
                string aanhef = currentUser.Title == 0 ? "heer " : "mevrouw ";
                ViewBag.Aanhef = aanhef + currentUser.LastName;
                using (var writer = new StringWriter())
                {
                    var context = new ViewContext(ControllerContext, view.View, ViewData, TempData, writer);
                    view.View.Render(context, writer);
                    writer.Flush();
                    content = writer.ToString();
                }

                MailMessage message = new MailMessage();
                MailAddress fromAddress = new MailAddress("ycsi@live.nl");
                message.From = fromAddress;
                message.To.Add(currentUser.Email);
                message.Subject = "Factuur";
                message.IsBodyHtml = true;
                message.Body = content;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.live.com";
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential
                ("emailadres", "wachtwoord");
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
            }

            return RedirectToAction("Index", "Store", null);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout()
        {
            string currentUser = HttpContext.User.Identity.GetUserId();
            ShoppingCart shoppingCart = null;
            List<ShoppingCartProduct> shoppingCartProducts = new List<ShoppingCartProduct>();

            shoppingCart = db.ShoppingCarts.Where(s => s.UserId == currentUser).FirstOrDefault();

            if (shoppingCart != null)
            {
                shoppingCartProducts = db.ShoppingCartProducts.Where(s => s.ShoppingCartId == shoppingCart.ShoppingCartId).OrderBy(p => p.ProductId).ToList();
            }

            decimal priceVat = 0.00M;
            foreach (ShoppingCartProduct shoppingCartProduct in shoppingCartProducts)
            {
                decimal temp = db.Products.Find(shoppingCartProduct.ProductId).Price * shoppingCartProduct.AmountOfProducts;
                priceVat += temp;
            }
            ViewBag.PriceVat = string.Format("{0:C}", priceVat);
            ViewBag.Vat = string.Format("{0:C}", (priceVat * 21) / 100);
            ViewBag.PriceNonVat = string.Format("{0:C}", (priceVat * 79) / 100);

            return View(shoppingCartProducts);
        }

        //// GET: Orders/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Orders/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "OrderId")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(order).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(order);
        //}

        //// GET: Orders/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Order order = db.Orders.Find(id);
        //    db.Orders.Remove(order);
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
