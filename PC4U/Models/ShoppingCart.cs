using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PC4U.Models
{
    public class ShoppingCart
    {
        public virtual int ShoppingCartId { get; set; }
        public virtual string UserId { get; set; }

        public virtual List<Product> Products { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}