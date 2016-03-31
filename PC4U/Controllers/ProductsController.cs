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
            List<Product> products = GetProductsIncludingImages();

            if (products == null)
            {
                return HttpNotFound();
            }

            return View(products);
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
        public ActionResult Create([Bind(Include = "Product, ImageStrings")] ProductCreateEditViewModel productCreateViewModel)
        {
            if (ModelState.IsValid)
            {
                if(productCreateViewModel.ImageStrings != null && productCreateViewModel.ImageStrings[0] != null)
                {
                    AddImageToProduct(productCreateViewModel);
                }
                db.Products.Add(productCreateViewModel.Product);
                db.SaveChanges();

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

            List<Product> products = GetProductsIncludingImages();

            if(products == null)
            {
                return HttpNotFound();
            }

            ProductCreateEditViewModel productEditViewModel = new ProductCreateEditViewModel()
            {
                Product = products.Where(p => p.ProductId == id).FirstOrDefault()
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
        public ActionResult Edit([Bind(Include = "Product, ImageStrings")] ProductCreateEditViewModel productEditViewModel)
        {
            if (productEditViewModel.ImageStrings != null && productEditViewModel.ImageStrings[0] != null)
            {
                AddImageToProduct(productEditViewModel);
            }
            db.Entry(productEditViewModel.Product).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
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

        private List<Product> GetProductsIncludingImages()
        {
            List<Product> products = db.Products.ToList();

            if(products != null)
            {
                foreach (Product p in products)
                {
                    p.Images = new List<Image>();
                    p.Images = db.ImageProducts.Where(ip => ip.ProductId == p.ProductId).Include(ip => ip.Image).Select(ip => ip.Image).ToList();
                }
                return products;
            }

            return null;
        }


        private void AddImageToProduct(ProductCreateEditViewModel productCreateEditViewModel)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (HttpPostedFileBase imageString in productCreateEditViewModel.ImageStrings)
                    {
                        byte[] encodedImage = Helper.ConvertHttpPostedFileBaseToByteArray(imageString);
                        int imageId;

                        // Add the image to the images table if it doesn't exist in the db yet.
                        if (!db.Images.Where(i => i.EncodedImage == encodedImage).Any())
                        {
                            // Create the new id of the images table.
                            imageId = db.Images.Any() ? db.Images.Max(c => c.ImageId) + 1 : 1;

                            db.Images.Add(new Image()
                            {
                                ImageId = imageId,
                                EncodedImage = encodedImage
                            });
                            db.SaveChanges();
                        }
                        else
                        {
                            // Get the already existing imageId that corresponds to the picture that is to be added.
                            imageId = db.Images.Where(i => i.EncodedImage == encodedImage).FirstOrDefault().ImageId;
                        }

                        // Add a record to the ImageProduct junction table.
                        db.ImageProducts.Add(new ImageProduct()
                        {
                            ImageId = imageId,
                            ProductId = productCreateEditViewModel.Product.ProductId
                        });
                        db.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }

        [HttpPost]
        public ActionResult RemoveImageFromProduct(int productId, int imageId)
        {
            // Get all the products that are related with the imageId.
            List<ImageProduct> imageProducts = db.ImageProducts.Where(ip => ip.ImageId == imageId).ToList();

            // Get THIS product that is related with the imageId.
            ImageProduct imageProduct = imageProducts.Where(ip => ip.ProductId == productId && ip.ImageId == imageId).FirstOrDefault();

            // Remove the image from the images table if it isn't related with any other product.
            if (imageProducts.Count == 1)
            {
                Image image = db.Images.Find(imageProduct.ImageId);
                db.Images.Remove(image);
            }

            // Remove the image-product relation.
            db.ImageProducts.Remove(imageProduct);
            db.SaveChanges();

            return new EmptyResult();
        }
    }
}
