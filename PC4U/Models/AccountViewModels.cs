using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PC4U.Models
{
    public enum Title
    {
        Man,
        Vrouw
    }

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
        [Display(Name = "Emailadres")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Emailadres")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Onthoud mij?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        const string VERPLICHT = "Dit veld is verplicht.";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title != TitleEnum.Man && Title != TitleEnum.Vrouw)
            {
                yield return new ValidationResult("De aanhef is vereist.", new[] { "Title" });
            }

            if (DateTime.Now.Year - BirthDate.Year < 18)
            {
                yield return new ValidationResult("U moet 18 of ouder zijn.", new[] { "BirthDate" });
            }
        }

        [Required(ErrorMessage = VERPLICHT)]
        [Display(Name = "Aanhef")]
        public virtual TitleEnum Title { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [StringLength(50, ErrorMessage = "Dit veld mag niet minder dan 2 of meer dan 50 karakters bevatten.", MinimumLength = 2)]
        [Display(Name = "Voornaam")]
        public virtual string FirstName { get; set; }

        [StringLength(10, ErrorMessage = "Dit veld mag niet meer dan 10 karakters bevatten.")]
        [Display(Name = "Tussenvoegsel")]
        public virtual string Insertion { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [StringLength(50, ErrorMessage = "Dit veld mag niet minder dan 2 of meer dan 50 karakters bevatten.", MinimumLength = 2)]
        [Display(Name = "Achternaam")]
        public virtual string LastName { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [Display(Name = "Land")]
        public virtual string Country { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [StringLength(10, ErrorMessage = "Dit veld mag niet minder dan 6 of meer dan 10 karakters bevatten.", MinimumLength = 6)]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postcode")]
        public virtual string PostalCode { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [Display(Name = "Huisnummer")]
        public virtual int HouseNumber { get; set; }

        [StringLength(20, ErrorMessage = "Dit veld mag niet meer dan 20 karakters bevatten.")]
        [Display(Name = "Huisnummertoevoeging")]
        public virtual string HouseNumberExtension { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [StringLength(50, ErrorMessage = "Dit veld mag niet minder dan 2 of meer dan 50 karakters bevatten.", MinimumLength = 2)]
        [Display(Name = "Straat")]
        public virtual string Street { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [StringLength(100, ErrorMessage = "Dit veld mag niet minder dan 6 of meer dan 100 karakters bevatten.", MinimumLength = 6)]
        [Display(Name = "Woonplaats")]
        public virtual string City { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Geboortedatum")]
        [DataType(DataType.Date)]
        public virtual DateTime BirthDate { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [StringLength(20, ErrorMessage = "Dit veld mag niet minder dan 7 of meer dan 20 karakters bevatten.", MinimumLength = 7)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefoonnummer")]
        public virtual string TelephoneNumber { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [EmailAddress]
        [Display(Name = "Emailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [StringLength(50, ErrorMessage = "Uw wachtwoord mag niet minder dan 6 of meer dan 50 karakters bevatten.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Required(ErrorMessage = VERPLICHT)]
        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet met elkaar overeen.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Emailadres")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Uw wachtwoord mag niet minder dan 6 of meer dan 50 karakters bevatten.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet met elkaar overeen.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Emailadres")]
        public string Email { get; set; }
    }
}