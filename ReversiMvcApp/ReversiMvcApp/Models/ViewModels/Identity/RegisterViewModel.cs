﻿using System.ComponentModel.DataAnnotations;

namespace ReversiMvcApp.Models.ViewModels.Identity
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at leats {2} and at max {1} characters long.", MinimumLength = 0)]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Naam { get; set; }
    }
}
