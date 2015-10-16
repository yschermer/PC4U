using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PC4U.Models
{ 
    public class Image
    {
        [Key]
        public virtual int ImageId { get; set; }
        public virtual int ProductId { get; set; }
        public virtual byte[] EncodedImage { get; set; }

        public virtual Product Product { get; set; }
    }
}