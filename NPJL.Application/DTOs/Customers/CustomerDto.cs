using System;
using System.Collections.Generic;
using System.Text;

namespace NPJL.Application.DTOs.Customers
{
    public sealed record CustomerDto
    {
        int Id { get; init; }
        string Name { get; init; } = String.Empty;
    }
}
