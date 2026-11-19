using System.ComponentModel.DataAnnotations;

namespace NPJL.Application.DTOs.Customers
{
    public sealed record UpdatePaymentDetailsDto
    {
        [Required(ErrorMessage = "Payment method is required")]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "Payment reference is required")]
        [StringLength(100, ErrorMessage = "Maximum 100 characters")]
        public string PaymentReference { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}