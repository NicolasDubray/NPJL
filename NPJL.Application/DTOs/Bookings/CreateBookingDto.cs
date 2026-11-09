using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace NPJL.Application.DTOs.Bookings
{
    public sealed record CreateBookingDto
    {
        [Required]
        [StringLength(50)]
        [MinLength(5)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int CustomerId { get; set; }
        //public List<CreateScheduledPeriodDto> ScheduledPeriods { get; set; } = new List<CreateScheduledPeriodDto>();
    }
}
