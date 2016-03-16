using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PC4U.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace PC4U.Helpers
{
    public class Helper
    {
        public enum RoleEnum
        {
            Administrator
        }

        public static void CreateUserByRole(RoleEnum role, ApplicationUser user, string password)
        {
            user.Id = Guid.NewGuid().ToString();

            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        manager.Create(user, password);
                        context.SaveChanges();

                        manager.AddToRole(user.Id, Enum.GetName(typeof(RoleEnum), (int)role));
                        context.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public static byte[] ConvertHttpPostedFileBaseToByteArray(HttpPostedFileBase httpPostedFileBase)
        {
            byte[] byteArray = new byte[httpPostedFileBase.ContentLength];
            httpPostedFileBase.InputStream.Read(byteArray, 0, byteArray.Length);
            return byteArray;
        }

        public static string ConvertByteArrayToString(byte[] byteArray)
        {
            return byteArray != null ? Convert.ToBase64String(byteArray) : string.Empty;
        }

        public static decimal GetTotalPrice(List<CartProduct> cartProducts, ApplicationDbContext db)
        {
            decimal priceVat = 0.00M;
            foreach (CartProduct cartProduct in cartProducts)
            {
                decimal temp = db.Products.Find(cartProduct.ProductId).Price * cartProduct.AmountOfProducts;
                priceVat += temp;
            }
            return priceVat;
        }
    }
}