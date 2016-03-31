using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PC4U.Models
{
    public class Image
    {
        [Key]
        public virtual int ImageId { get; set; }
        public virtual byte[] EncodedImage { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}