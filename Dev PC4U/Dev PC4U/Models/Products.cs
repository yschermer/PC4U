using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dev_PC4U.Models
{
    public class Products
    {
        [Key]
        public int ProductId { get; set; }

        [Display(Name = "Artikel")]
        public string ProductName { get; set; }
    }
}