using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PC4U.Models;
using PC4U.Helpers;

namespace PC4U.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories.OrderBy(g => g.CategoryName), "CategoryId", "CategoryName");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product, Images, SelectedImages")] ProductCreateEditViewModel productCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(productCreateViewModel.Product);
                db.SaveChanges();

                if (productCreateViewModel.Images != null && productCreateViewModel.Images[0] != null)
                {
                    foreach (var image in productCreateViewModel.Images)
                    {
                        Image imageObject = new Image();
                        imageObject.EncodedImage = Helper.ConvertHttpPostedFileBaseToByteArray(image);
                        imageObject.ProductId = productCreateViewModel.Product.ProductId;
                        db.Images.Add(imageObject);
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(productCreateViewModel);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCreateEditViewModel productEditViewModel = new ProductCreateEditViewModel()
            {
                Product = db.Products.Find(id)
            };
            if (productEditViewModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Categories = new SelectList(db.Categories.OrderBy(g => g.CategoryName), "CategoryId", "CategoryName", productEditViewModel.Product.CategoryId);

            return View(productEditViewModel);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product, Images, SelectedImages")] ProductCreateEditViewModel productEditViewModel)
        {
            db.Entry(productEditViewModel.Product).State = EntityState.Modified;
            db.SaveChanges();

            if (productEditViewModel.Images != null && productEditViewModel.Images[0] != null)
            {
                foreach (var image in productEditViewModel.Images)
                {
                    Image imageInstance = new Image();
                    imageInstance.EncodedImage = Helper.ConvertHttpPostedFileBaseToByteArray(image);
                    imageInstance.ProductId = productEditViewModel.Product.ProductId;
                    db.Images.Add(imageInstance);
                    db.SaveChanges();
                }
            }

            if (productEditViewModel.SelectedImages != null)
            {
                foreach (int imageId in productEditViewModel.SelectedImages)
                {
                    Image imageInstance = db.Images.Find(imageId);
                    db.Images.Remove(imageInstance);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");

            ViewBag.Categories = new SelectList(db.Categories.OrderBy(g => g.CategoryName), "CategoryId", "CategoryName", productEditViewModel.Product.CategoryId);
            return View(productEditViewModel);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
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
