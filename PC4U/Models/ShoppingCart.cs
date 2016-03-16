using System.Collections.Generic;

namespace PC4U.Models
{
    public class ShoppingCart
    {
        public virtual int ShoppingCartId { get; set; }
        public virtual string UserId { get; set; }
        public virtual StatusEnum Status { get; set; }

        public virtual List<Product> Products { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}