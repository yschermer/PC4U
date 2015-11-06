using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PC4U.Models
{
    public enum StatusEnum
    {
        Unordered,
        Ordered
    }

    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Status = StatusEnum.Unordered;
        }

        public virtual int ShoppingCartId { get; set; }
        public virtual string UserId { get; set; }
        public virtual StatusEnum Status { get; set; }

        public virtual List<Product> Products { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}