using System.ComponentModel.DataAnnotations;

namespace NPJL.Application.DTOs.Customers
{
    public sealed record UpdateCustomerProfileDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telephone is required")]
        [StringLength(30, ErrorMessage = "Maximum 30 characters")]
        public string Telephone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(250, ErrorMessage = "Maximum 250 characters")]
        public string Address { get; set; } = string.Empty;
    }
}