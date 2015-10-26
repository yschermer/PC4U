using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
        [Display(Name = "Aantal")]
        public virtual int AmountOfProducts { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual Product Product { get; set; }
    }
}