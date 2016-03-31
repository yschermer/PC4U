using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PC4U.Models
{
    public class CartProduct
    {
        public CartProduct()
        {
            AmountOfProducts = 1;
        }

        [Key, Column(Order = 0)]
        public virtual int CartId { get; set; }

        [Key, Column(Order = 1)]
        public virtual int ProductId { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Range(1, 1000, ErrorMessageResourceName = "RangeInt", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "Amount", ResourceType = typeof(Resources.ModelResources))]
        public virtual int AmountOfProducts { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}