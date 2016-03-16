using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PC4U.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
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
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Emailaddress")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Emailaddress")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        const string REQUIRED_TEXT = "This field is required.";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title != TitleEnum.MALE && Title != TitleEnum.FEMALE)
            {
                yield return new ValidationResult(REQUIRED_TEXT, new[] { "Title" });
            }

            if (DateTime.Now.Year - BirthDate.Year < 18)
            {
                yield return new ValidationResult("You must be above 18 years old.", new[] { "BirthDate" });
            }
        }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [Display(Name = "Title")]
        public virtual TitleEnum Title { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [StringLength(50, ErrorMessage = "This field may not contain less than 2 and more than 50 characters.", MinimumLength = 2)]
        [Display(Name = "First name")]
        public virtual string FirstName { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [StringLength(50, ErrorMessage = "This field may not contain less than 2 and more than 50 characters.", MinimumLength = 2)]
        [Display(Name = "Last name")]
        public virtual string LastName { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [Display(Name = "Country")]
        public virtual string Country { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [StringLength(10, ErrorMessage = "This field may not contain less than 6 and more than 10 characters.", MinimumLength = 6)]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postal code")]
        public virtual string PostalCode { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [Display(Name = "Housenumber")]
        public virtual int HouseNumber { get; set; }

        [StringLength(10, ErrorMessage = "This field may not contain more than 10 characters.")]
        [Display(Name = "Housenumber suffix")]
        public virtual string HouseNumberExtension { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [StringLength(50, ErrorMessage = "This field may not contain less than 2 and more than 50 characters.", MinimumLength = 2)]
        [Display(Name = "Street")]
        public virtual string Street { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [StringLength(100, ErrorMessage = "This field may not contain less than 6 and more than 100 characters.", MinimumLength = 6)]
        [Display(Name = "City")]
        public virtual string City { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public virtual DateTime BirthDate { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [StringLength(20, ErrorMessage = "This field may not contain less than 7 and more than 20 characters.", MinimumLength = 7)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phonenumber")]
        public virtual string TelephoneNumber { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [EmailAddress]
        [Display(Name = "Emailaddress")]
        public string Email { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [StringLength(50, ErrorMessage = "This field may not contain less than 6 and more than 50 characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = REQUIRED_TEXT)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Emailaddress")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "This field may not contain less than 6 and more than 50 characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Emailaddress")]
        public string Email { get; set; }
    }
}