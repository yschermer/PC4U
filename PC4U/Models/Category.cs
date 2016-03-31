using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PC4U.Models
{
    public class Category
    {
        public virtual int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "Category", ResourceType = typeof(Resources.ModelResources))]
        public virtual string CategoryName { get; set; }

        public virtual List<Product> Product { get; set; }
    }
}