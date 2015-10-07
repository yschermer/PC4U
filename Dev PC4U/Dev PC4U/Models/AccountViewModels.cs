using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dev_PC4U.Models
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
        [Required]
        [EmailAddress]
        [Display(Name = "Emailadres")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Uw wachtwoord mag niet minder dan 6 of meer dan 50 karakters bevatten.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet met elkaar overeen.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Aanhef")]
        public string Title { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Uw voornaam mag niet minder dan 2 of meer dan 50 karakters bevatten.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Range(0, 10)]
        [Display(Name = "Tussenvoegsel")]
        public string Insertion { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Uw achternaam mag niet minder dan 2 of meer dan 50 karakters bevatten.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Achternaam")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Land")]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [Display(Name = "Postcode")]
        public string PostalCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Huisnummer")]
        public string HouseNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Huisnummertoevoeging")]
        public string HouseNumberExtension { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Uw adres mag niet minder dan 2 of meer dan 50 karakters bevatten.", MinimumLength = 2)]
        [DataType(DataType.Text)]
        [Display(Name = "Straat")]
        public string Street { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Uw woonplaats mag niet minder dan 6 of meer dan 100 karakters bevatten.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Woonplaats")]
        public string City { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true,
               DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Geboortedatum")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefoonnummer")]
        public string TelephoneNumber { get; set; }
    }
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

