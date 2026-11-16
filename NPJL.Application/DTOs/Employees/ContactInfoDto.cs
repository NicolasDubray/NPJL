using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Employees
{
    // DTO för att visa kontaktuppgifter till en anställd.
    public sealed record ContactInfoDto
    {
        public int Id { get; init; }

        public string Address { get; init; } = string.Empty;

        public string Telephone { get; init; } = string.Empty;

        public string Email { get; init; } = string.Empty;
    }
}