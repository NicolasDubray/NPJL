using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NPJL.Application.DTOs.Authentication
{
    public class CustomerRegistrationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telephone is required.")]
        [MaxLength(30)]
        public string Telephone { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        public bool IsCompany { get; set; }

        [MaxLength(200)]
        public string? CompanyName { get; set; }
    }
}
