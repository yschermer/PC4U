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

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Category")]
        public virtual string CategoryName { get; set; }

        public virtual List<Product> Product { get; set; }
    }
}