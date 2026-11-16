using NPJL.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace NPJL.Application.DTOs.Requests
{
    public sealed record CreateRequestDto
    {
        public int CustomerId { get; set; }
        public int BookingId { get; set; }
        public RequestType Type { get; set; }

        [Required(ErrorMessage = "Please describe your request")]
        [StringLength(1000, ErrorMessage = "Maximum 1000 characters")]
        public string Description { get; set; } = string.Empty;
    }
}