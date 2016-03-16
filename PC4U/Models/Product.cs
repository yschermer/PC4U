using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PC4U.Models
{
    public class Product
    {
        const string REQUIRED_TEXT = "This field is required.";

        public virtual int ProductId { get; set; }
        public virtual int CategoryId { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [Display(Name = "Product")]
        public virtual string ProductName { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [DataType(DataType.Currency)]
        [Display(Name = "Price")]
        public virtual decimal Price { get; set; }

        [Display(Name = "Image")]
        public virtual List<Image> Image { get; set; }
        public virtual List<Cart> Carts { get; set; }
        public virtual Category Category { get; set; }
    }

    public class ProductCreateEditViewModel
    {
        public virtual Product Product { get; set; }
        public virtual List<HttpPostedFileBase> Images { get; set; }
        public virtual int[] SelectedImages { get; set; }
    }
}