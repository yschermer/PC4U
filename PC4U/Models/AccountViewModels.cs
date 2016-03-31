using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PC4U.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.ModelResources))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        public string Provider { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.ModelResources))]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.ModelResources))]
        [EmailAddress]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.ModelResources))]
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Resources.ModelResources))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title != TitleEnum.MALE && Title != TitleEnum.FEMALE)
            {
                yield return new ValidationResult(Resources.ModelResources.Required, new[] { "Title" });
            }

            if (DateTime.Now.Year - BirthDate.Year < 18)
            {
                yield return new ValidationResult(Resources.ModelResources.TooYoung, new[] { "BirthDate" });
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "Title", ResourceType = typeof(Resources.ModelResources))]
        public virtual TitleEnum Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [StringLength(50, ErrorMessageResourceName = "RangeString", ErrorMessageResourceType = typeof(Resources.ModelResources), MinimumLength = 2)]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.ModelResources))]
        public virtual string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [StringLength(50, ErrorMessageResourceName = "RangeString", ErrorMessageResourceType = typeof(Resources.ModelResources), MinimumLength = 2)]
        [Display(Name = "LastName", ResourceType = typeof(Resources.ModelResources))]
        public virtual string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "Country", ResourceType = typeof(Resources.ModelResources))]
        public virtual string Country { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [StringLength(10, ErrorMessageResourceName = "RangeString", ErrorMessageResourceType = typeof(Resources.ModelResources), MinimumLength = 6)]
        [DataType(DataType.PostalCode)]
        [Display(Name = "PostalCode", ResourceType = typeof(Resources.ModelResources))]
        public virtual string PostalCode { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "HouseNumber", ResourceType = typeof(Resources.ModelResources))]
        public virtual int HouseNumber { get; set; }

        [StringLength(10, ErrorMessageResourceName = "MaxString", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [Display(Name = "HouseNumberSuffix", ResourceType = typeof(Resources.ModelResources))]
        public virtual string HouseNumberExtension { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [StringLength(50, ErrorMessageResourceName = "RangeString", ErrorMessageResourceType = typeof(Resources.ModelResources), MinimumLength = 2)]
        [Display(Name = "Street", ResourceType = typeof(Resources.ModelResources))]
        public virtual string Street { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [StringLength(100, ErrorMessageResourceName = "RangeString", ErrorMessageResourceType = typeof(Resources.ModelResources), MinimumLength = 6)]
        [Display(Name = "City", ResourceType = typeof(Resources.ModelResources))]
        public virtual string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "BirthDate", ResourceType = typeof(Resources.ModelResources))]
        [DataType(DataType.Date)]
        public virtual DateTime BirthDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [StringLength(20, ErrorMessageResourceName = "RangeString", ErrorMessageResourceType = typeof(Resources.ModelResources), MinimumLength = 7)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.ModelResources))]
        public virtual string TelephoneNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [EmailAddress]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.ModelResources))]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [StringLength(50, ErrorMessageResourceName = "RangeString", ErrorMessageResourceType = typeof(Resources.ModelResources), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.ModelResources))]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.ModelResources))]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [EmailAddress]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.ModelResources))]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [StringLength(50, ErrorMessageResourceName = "RangeString", ErrorMessageResourceType = typeof(Resources.ModelResources), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.ModelResources))]
        public string Password { get; set; }

        [Compare("Password", ErrorMessageResourceName = "FalsePasswordMatch", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.ModelResources))]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ModelResources))]
        [EmailAddress]
        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.ModelResources))]
        public string Email { get; set; }
    }
}