using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PC4U.Models
{
    public class ImageProduct
    {
        [Key, Column(Order = 0)]
        public virtual int ImageId { get; set; }

        [Key, Column(Order = 1)]
        public virtual int ProductId { get; set; }

        public virtual Image Image { get; set; }
        public virtual Product Product { get; set; }
    }
}