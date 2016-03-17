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
using System.IO;
using System.Net.Mail;
using System.Net;

namespace PC4U.Controllers
{
    public class CartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

        // GET: Carts
        public ActionResult Index()
        {
            List<CartProduct> cartProducts = GetCartProductsByStatus(StatusEnum.PENDING);

            if (cartProducts == null || cartProducts.Count == 0)
            {
                return View("Empty");
            }
            else
            {
                // Calculate the total price with vat, vat, and the total price without vat
                ViewBag.PriceVat = string.Format("{0:C}", Helper.GetTotalPrice(cartProducts, db));
                ViewBag.Vat = string.Format("{0:C}", (Helper.GetTotalPrice(cartProducts, db) * 21) / 100);
                ViewBag.PriceNonVat = string.Format("{0:C}", (Helper.GetTotalPrice(cartProducts, db) * 79) / 100);
            }
            return View(cartProducts);
        }

        public List<CartProduct> GetCartProductsByStatus(StatusEnum status)
        {
            List<CartProduct> cartProducts = new List<CartProduct>();

            // Check if it is a user or a visitor
            if (user != null)
            {
                Cart cart = db.Carts.Where(c => c.UserId == user.Id && c.Status == status).FirstOrDefault();

                // Check if the user already has a cart in db
                if (cart != null)
                {
                    cartProducts = db.CartProducts.Where(c => c.CartId == cart.CartId).OrderBy(p => p.ProductId).ToList();
                }
            }
            else
            {
                cartProducts = (List<CartProduct>)Session["Cart"];

                // Check if the visitor already has a cart in cache
                if (cartProducts != null)
                {
                    foreach (CartProduct c in cartProducts)
                    {
                        c.Product = db.Products.Where(p => p.ProductId == c.ProductId).FirstOrDefault();
                    }
                }
            }
            return cartProducts;
        }

        // POST: Carts/Create
        [HttpPost]
        public ActionResult AddToCart([Bind(Include = "CartId, ProductId, AmountOfProducts")] CartProduct cartProduct)
        {
            // Check if it is a user or a visitor, and if the amount of products is more than 0.
            if (cartProduct.AmountOfProducts > 0)
            {
                if (user != null)
                {
                    AddToUserCart(cartProduct);
                }
                else
                {
                    AddToVisitorCart(cartProduct);
                }
            }
            return RedirectToAction("Index", "Store", null);
        }

        private void AddToUserCart(CartProduct input)
        {
            Cart cart = db.Carts.Where(c => c.UserId == user.Id && c.Status == StatusEnum.PENDING).FirstOrDefault();

            // Check if the user already has a cart in db
            if (cart != null && cart.Status == StatusEnum.PENDING)
            {
                CartProduct cartProduct = new CartProduct()
                {
                    CartId = cart.CartId,
                    ProductId = input.ProductId,
                    AmountOfProducts = input.AmountOfProducts
                };

                // Check if the product already exists in the cart
                if (db.CartProducts.Where(c => c.ProductId == cartProduct.ProductId && c.CartId == cart.CartId).Any())
                {
                    db.Entry(cartProduct).State = EntityState.Modified;
                }
                else
                {
                    db.CartProducts.Add(cartProduct);
                }
            }
            else
            {
                cart = new Cart()
                {
                    CartId = db.Carts.Any() ? db.Carts.Max(c => c.CartId) : 1,
                    UserId = user.Id,
                    Status = StatusEnum.PENDING
                };

                CartProduct cartProduct = new CartProduct()
                {
                    Cart = cart,
                    CartId = cart.CartId,
                    ProductId = input.ProductId,
                    AmountOfProducts = input.AmountOfProducts
                };
                db.Carts.Add(cart);
                db.CartProducts.Add(cartProduct);
            }
            db.SaveChanges();
        }

        private void AddToVisitorCart(CartProduct input)
        {
            List<CartProduct> cartProducts = new List<CartProduct>();
            var temp = (List<CartProduct>)Session["Cart"];
            if (Session["Cart"] == null || temp.Count == 0)
            {
                cartProducts.Add(input);
                Session["Cart"] = cartProducts;
            }
            else
            {
                cartProducts.AddRange((List<CartProduct>)Session["Cart"]);
                if (!cartProducts.Where(c => c.ProductId == input.ProductId).Any())
                {
                    cartProducts.Add(input);
                }
                Session["Cart"] = cartProducts;
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PreCheckout()
        {
            List<CartProduct> cartProducts = new List<CartProduct>();

            Cart cart = db.Carts.Where(c => c.UserId == user.Id && c.Status == StatusEnum.PENDING).FirstOrDefault();

            // Check if the user already has a cart
            if (cart != null)
            {
                cartProducts = db.CartProducts.Where(c => c.CartId == cart.CartId).OrderBy(p => p.ProductId).ToList();
            }

            ViewBag.PriceVat = string.Format("{0:C}", Helper.GetTotalPrice(cartProducts, db));
            ViewBag.Vat = string.Format("{0:C}", (Helper.GetTotalPrice(cartProducts, db) * 21) / 100);
            ViewBag.PriceNonVat = string.Format("{0:C}", (Helper.GetTotalPrice(cartProducts, db) * 79) / 100);

            return View(cartProducts);
        }

        // POST: Carts/Checkout
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout([Bind(Include = "SendMail")] bool sendMail)
        {
            Cart cart = db.Carts.Include(c => c.User).Where(c => c.UserId == user.Id && c.Status == StatusEnum.PENDING).FirstOrDefault();
            cart.Status = StatusEnum.PAID;

            List<CartProduct> cartProducts = db.CartProducts.Include(c => c.Product).Where(c => c.CartId == cart.CartId).ToList();
            ViewBag.PriceVat = string.Format("{0:C}", Helper.GetTotalPrice(cartProducts, db));
            ViewBag.Vat = string.Format("{0:C}", (Helper.GetTotalPrice(cartProducts, db) * 21) / 100);
            ViewBag.PriceNonVat = string.Format("{0:C}", (Helper.GetTotalPrice(cartProducts, db) * 79) / 100);

            db.Entry(cart).State = EntityState.Modified;
            db.SaveChanges();
            
            // Works, but an emailaddress and password is required
            // if (sendMail)
            // {
            //     SendMail(cartProducts);
            // }
            
            return RedirectToAction("Index", "Store", null);
        }

        private void SendMail(List<CartProduct> cartProducts)
        {
            string relativePath = "~/Views/Orders/Invoice.cshtml";
            var content = string.Empty;
            var view = ViewEngines.Engines.FindView(ControllerContext, relativePath, null);
            foreach (CartProduct c in cartProducts)
            {
                c.Product = db.Products.Find(c.ProductId);
            }
            ViewData.Model = cartProducts;
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

        // POST: Carts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CartId, ProductId, AmountOfProducts")] CartProduct cartProduct)
        {
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    Cart cart = db.Carts.Where(c => c.UserId == user.Id && c.Status == StatusEnum.PENDING).FirstOrDefault();
                    cartProduct.CartId = cart.CartId;
                    db.Entry(cartProduct).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    List<CartProduct> cartProducts = (List<CartProduct>)Session["Cart"];

                    // Must re-add the product, because the source is the Session.
                    CartProduct cartProductToEdit = cartProducts.Where(c => c.ProductId == cartProduct.ProductId && c.CartId == cartProduct.CartId).FirstOrDefault();
                    cartProducts.Remove(cartProductToEdit);
                    cartProducts.Add(cartProduct);
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Carts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "CartId, ProductId")] CartProduct cartProduct)
        {
            if(user != null)
            {
                Cart cart = db.Carts.Where(c => c.UserId == user.Id && c.Status == StatusEnum.PENDING).FirstOrDefault();
                cartProduct = db.CartProducts.Where(c => c.CartId == cart.CartId && c.ProductId == cartProduct.ProductId).FirstOrDefault();
                db.CartProducts.Remove(cartProduct);
                db.SaveChanges();
            }
            else
            {
                List<CartProduct> cartProducts = (List<CartProduct>)Session["Cart"];
                CartProduct cartProductToRemove = cartProducts.Where(c => c.ProductId == cartProduct.ProductId && c.CartId == cartProduct.CartId).FirstOrDefault();
                cartProducts.Remove(cartProductToRemove);
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
