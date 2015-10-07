using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PC4U.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Display(Name = "Artikel")]
        public string ProductName { get; set; }
    }
}