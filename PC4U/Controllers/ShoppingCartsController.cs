using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PC4U.Models;
using Microsoft.AspNet.Identity;
using PC4U.Helpers;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace PC4U.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        // GET: ShoppingCarts
        public ActionResult Index()
        {
            List<ShoppingCartProduct> shoppingCartProducts = GetShoppingCartProductsByStatus(StatusEnum.PENDING);

            if (shoppingCartProducts == null || shoppingCartProducts.Count == 0)
            {
                return View("Empty");
            }
            else
            {
                // Calculate the total price with vat, vat, and the total price without vat
                ViewBag.PriceVat = string.Format("{0:C}", Helper.GetTotalPrice(shoppingCartProducts, db));
                ViewBag.Vat = string.Format("{0:C}", (Helper.GetTotalPrice(shoppingCartProducts, db) * 21) / 100);
                ViewBag.PriceNonVat = string.Format("{0:C}", (Helper.GetTotalPrice(shoppingCartProducts, db) * 79) / 100);
            }
            return View(shoppingCartProducts);
        }

        public List<ShoppingCartProduct> GetShoppingCartProductsByStatus(StatusEnum status)
        {
            List<ShoppingCartProduct> shoppingCartProducts = new List<ShoppingCartProduct>();

            // Check if it is a user or a visitor
            if (user != null)
            {
                ShoppingCart shoppingCart = db.ShoppingCarts.Where(s => s.UserId == user.Id && s.Status == status).FirstOrDefault();

                // Check if the user already has a cart in db
                if (shoppingCart != null)
                {
                    shoppingCartProducts = db.ShoppingCartProducts.Where(s => s.ShoppingCartId == shoppingCart.ShoppingCartId).OrderBy(p => p.ProductId).ToList();
                }
            }
            else
            {
                shoppingCartProducts = (List<ShoppingCartProduct>)Session["ShoppingCart"];

                // Check if the visitor already has a cart in cache
                if (shoppingCartProducts != null)
                {
                    foreach (ShoppingCartProduct s in shoppingCartProducts)
                    {
                        s.Product = db.Products.Where(p => p.ProductId == s.ProductId).FirstOrDefault();
                    }
                }
            }
            return shoppingCartProducts;
        }

        // POST: ShoppingCarts/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "ShoppingCartId, ProductId, AmountOfProducts")] ShoppingCartProduct shoppingCartProduct)
        {
            // Check if it is a user or a visitor, and if the amount of products is more than 0.
            if (shoppingCartProduct.AmountOfProducts > 0)
            {
                if (user != null)
                {
                    CreateUserShoppingCart(shoppingCartProduct);
                }
                else
                {
                    CreateVisitorShoppingCart(shoppingCartProduct);
                }
            }
            return RedirectToAction("Index", "Store", null);
        }

        private void CreateUserShoppingCart(ShoppingCartProduct input)
        {
            ShoppingCart shoppingCart = db.ShoppingCarts.Where(s => s.UserId == user.Id && s.Status == StatusEnum.PENDING).FirstOrDefault();

            // Check if the user already has a cart in db
            if (shoppingCart != null && shoppingCart.Status == StatusEnum.PENDING)
            {
                ShoppingCartProduct shoppingCartProduct = new ShoppingCartProduct()
                {
                    ShoppingCartId = shoppingCart.ShoppingCartId,
                    ProductId = input.ProductId,
                    AmountOfProducts = input.AmountOfProducts
                };

                // Check if the product already exists in the cart
                if (db.ShoppingCartProducts.Where(s => s.ProductId == shoppingCartProduct.ProductId && s.ShoppingCartId == shoppingCart.ShoppingCartId).Any())
                {
                    db.Entry(shoppingCartProduct).State = EntityState.Modified;
                }
                else
                {
                    db.ShoppingCartProducts.Add(shoppingCartProduct);
                }
            }
            else
            {
                shoppingCart = new ShoppingCart()
                {
                    ShoppingCartId = db.ShoppingCarts.Any() ? db.ShoppingCarts.Max(s => s.ShoppingCartId) : 1,
                    UserId = user.Id,
                    Status = StatusEnum.PENDING
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

        private void CreateVisitorShoppingCart(ShoppingCartProduct input)
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout()
        {
            ShoppingCart shoppingCart = null;
            List<ShoppingCartProduct> shoppingCartProducts = new List<ShoppingCartProduct>();

            shoppingCart = db.ShoppingCarts.Where(s => s.UserId == user.Id && s.Status == StatusEnum.PENDING).FirstOrDefault();

            if (shoppingCart != null)
            {
                shoppingCartProducts = db.ShoppingCartProducts.Where(s => s.ShoppingCartId == shoppingCart.ShoppingCartId).OrderBy(p => p.ProductId).ToList();
            }

            ViewBag.PriceVat = string.Format("{0:C}", Helper.GetTotalPrice(shoppingCartProducts, db));
            ViewBag.Vat = string.Format("{0:C}", (Helper.GetTotalPrice(shoppingCartProducts, db) * 21) / 100);
            ViewBag.PriceNonVat = string.Format("{0:C}", (Helper.GetTotalPrice(shoppingCartProducts, db) * 79) / 100);

            return View(shoppingCartProducts);
        }

        // POST: Orders/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder([Bind(Include = "SendMail")] bool sendMail)
        {
            ShoppingCart shoppingCart = db.ShoppingCarts.Include(s => s.User).Where(s => s.UserId == user.Id && s.Status == StatusEnum.PENDING).FirstOrDefault();
            shoppingCart.Status = StatusEnum.PAID;

            List<ShoppingCartProduct> shoppingCartProducts = db.ShoppingCartProducts.Include(s => s.Product).Where(s => s.ShoppingCartId == shoppingCart.ShoppingCartId).ToList();
            ViewBag.PriceVat = string.Format("{0:C}", Helper.GetTotalPrice(shoppingCartProducts, db));
            ViewBag.Vat = string.Format("{0:C}", (Helper.GetTotalPrice(shoppingCartProducts, db) * 21) / 100);
            ViewBag.PriceNonVat = string.Format("{0:C}", (Helper.GetTotalPrice(shoppingCartProducts, db) * 79) / 100);

            db.Entry(shoppingCart).State = EntityState.Modified;
            db.SaveChanges();
            
            // Works, but an emailaddress and password is required
            // if (sendMail)
            // {
            //     SendMail(shoppingCartProducts);
            // }
            
            return RedirectToAction("Index", "Store", null);
        }

        private void SendMail(List<ShoppingCartProduct> shoppingCartProducts)
        {
            string relativePath = "~/Views/Orders/Invoice.cshtml";
            var content = string.Empty;
            var view = ViewEngines.Engines.FindView(ControllerContext, relativePath, null);
            foreach (ShoppingCartProduct s in shoppingCartProducts)
            {
                s.Product = db.Products.Find(s.ProductId);
            }
            ViewData.Model = shoppingCartProducts;
            string title = user.Title == 0 ? "Mr. " : "Ms. ";
            ViewBag.Aanhef = title + user.LastName;
            using (var writer = new StringWriter())
            {
                var context = new ViewContext(ControllerContext, view.View, ViewData, TempData, writer);
                view.View.Render(context, writer);
                writer.Flush();
                content = writer.ToString();
            }

            MailMessage message = new MailMessage();
            MailAddress fromAddress = new MailAddress("emailaddress");
            message.From = fromAddress;
            message.To.Add(user.Email);
            message.Subject = "Invoice";
            message.IsBodyHtml = true;
            message.Body = content;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.live.com";
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential
            ("emailaddress", "password");
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);
        }

        // POST: ShoppingCarts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ShoppingCartId, ProductId, AmountOfProducts")] ShoppingCartProduct shoppingCartProduct)
        {
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    ShoppingCart shoppingCart = db.ShoppingCarts.Where(s => s.UserId == user.Id && s.Status == StatusEnum.PENDING).FirstOrDefault();
                    shoppingCartProduct.ShoppingCartId = shoppingCart.ShoppingCartId;
                    db.Entry(shoppingCartProduct).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    List<ShoppingCartProduct> shoppingCartProducts = (List<ShoppingCartProduct>)Session["ShoppingCart"];

                    // Must re-add the product, because the source is the Session.
                    ShoppingCartProduct shopppingCartProductToEdit = shoppingCartProducts.Where(s => s.ProductId == shoppingCartProduct.ProductId && s.ShoppingCartId == shoppingCartProduct.ShoppingCartId).FirstOrDefault();
                    shoppingCartProducts.Remove(shopppingCartProductToEdit);
                    shoppingCartProducts.Add(shoppingCartProduct);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: ShoppingCarts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "ShoppingCartId, ProductId")] ShoppingCartProduct shoppingCartProduct)
        {
            if(user != null)
            {
                ShoppingCart shoppingCart = db.ShoppingCarts.Where(s => s.UserId == user.Id && s.Status == StatusEnum.PENDING).FirstOrDefault();
                shoppingCartProduct = db.ShoppingCartProducts.Where(s => s.ShoppingCartId == shoppingCart.ShoppingCartId && s.ProductId == shoppingCartProduct.ProductId).FirstOrDefault();
                db.ShoppingCartProducts.Remove(shoppingCartProduct);
                db.SaveChanges();
            }
            else
            {
                List<ShoppingCartProduct> shoppingCartProducts = (List<ShoppingCartProduct>)Session["ShoppingCart"];
                ShoppingCartProduct shoppingCartProductToRemove = shoppingCartProducts.Where(s => s.ProductId == shoppingCartProduct.ProductId && s.ShoppingCartId == shoppingCartProduct.ShoppingCartId).FirstOrDefault();
                shoppingCartProducts.Remove(shoppingCartProductToRemove);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
