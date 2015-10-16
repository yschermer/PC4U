using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PC4U.Models
{
    public class Category
    {
        const string VERPLICHT = "Dit veld is verplicht.";

        public virtual int CategoryId { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [Display(Name = "Categorie")]
        public virtual string CategoryName { get; set; }

        public virtual List<Product> Product { get; set; }
    }
}