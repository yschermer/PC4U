using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PC4U.Models
{
    public class ShoppingCartProduct
    {
        public ShoppingCartProduct()
        {
            AmountOfProducts = 1;
        }

        [Key, Column(Order = 0)]
        public virtual int ShoppingCartId { get; set; }

        [Key, Column(Order = 1)]
        public virtual int ProductId { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [Range(1, 1000, ErrorMessage = "The amount cannot be lower than 1 and more than 1000.")]
        public virtual int AmountOfProducts { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual Product Product { get; set; }
    }
}