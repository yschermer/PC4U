using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace PC4U.Helpers
{
    public class CustomConverter
    {
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

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        //public static List<string> GetImageList(List<byte[]> images)
        //{
        //    List<string> imageList = new List<string>();

        //    if (images != null)
        //    {
        //        foreach (byte[] image in images)
        //        {
        //            if (image != null)
        //            {
        //                imageList.Add(Convert.ToBase64String(image));
        //            }
        //        }
        //    }

        //    return imageList;
        //}
    }
}