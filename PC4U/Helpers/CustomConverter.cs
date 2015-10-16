using System;
using System.Collections.Generic;
using System.Linq;
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