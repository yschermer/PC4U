using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PC4U.Models;
using PagedList;

namespace PC4U.Controllers
{
    public class StoreController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Store
        public ActionResult Index(int? categoryId, string order, int pageNumber = 1, int pageSize = 9)
        {
            List<Product> products = new List<Product>();
            string[] orders = { "Asc", "Desc" };

            // Sort by category
            if (categoryId == null)
            {
                products = db.Products.ToList();
            }
            else if (categoryId > 0)
            {
                products = db.Products.Where(p => p.CategoryId == categoryId).ToList();
            }

            if (products.Count == 0) { return HttpNotFound(); }

            // Sort by order
            if (order == orders[0])
            {
                products = products.OrderBy(p => p.Price).ToList();
            }
            else if (order == orders[1])
            {
                products = products.OrderByDescending(p => p.Price).ToList();
            }

            ViewBag.Categories = db.Categories.ToList();

            return View("Index", new PagedList<Product>(products, pageNumber, pageSize));
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
    }
}
