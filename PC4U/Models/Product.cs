using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PC4U.Models
{
    public class Product
    {
        public virtual int ProductId { get; set; }
        public virtual int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "Product", ResourceType = typeof(Resources.ModelResources))]
        public virtual string ProductName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [DataType(DataType.Currency)]
        [Display(Name = "Price", ResourceType = typeof(Resources.ModelResources))]
        public virtual decimal Price { get; set; }

        [Display(Name = "Images", ResourceType = typeof(Resources.ModelResources))]
        public virtual List<Image> Images { get; set; }
        public virtual List<Cart> Carts { get; set; }
        public virtual Category Category { get; set; }
    }

    public class ProductCreateEditViewModel
    {
        public virtual Product Product { get; set; }
        public virtual List<HttpPostedFileBase> ImageStrings { get; set; }
    }
}