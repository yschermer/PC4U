using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PC4U.Models
{
    public class Order
    {
        public virtual int OrderId { get; set; }
        public virtual string CustomerId { get; set; }
        public virtual int ShoppingCartId { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
    }
}