using System.Collections.Generic;

namespace PC4U.Models
{
    public class Cart
    {
        public virtual int CartId { get; set; }
        public virtual string UserId { get; set; }
        public virtual StatusEnum Status { get; set; }

        public virtual List<Product> Products { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}