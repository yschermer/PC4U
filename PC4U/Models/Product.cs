using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PC4U.Models
{
    public class Product
    {
        const string VERPLICHT = "Dit veld is verplicht.";

        public virtual int ProductId { get; set; }
        public virtual int CategoryId { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [Display(Name = "Artikel")]
        public virtual string ProductName { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [DataType(DataType.Currency)]
        [Display(Name = "Prijs")]
        public virtual decimal Price { get; set; }

        [Display(Name = "Afbeelding")]
        public virtual List<Image> Image { get; set; }
        public virtual List<ShoppingCart> ShoppingCarts { get; set; }
        public virtual Category Category { get; set; }
    }

    public class ProductCreateEditViewModel
    {
        public virtual Product Product { get; set; }
        public virtual List<HttpPostedFileBase> Images { get; set; }
        public virtual int[] SelectedImages { get; set; }
    }
}