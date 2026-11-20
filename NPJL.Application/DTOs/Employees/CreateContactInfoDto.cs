namespace NPJL.Application.DTOs.Employees
{
    // DTO for creating contact information for an employee.
    public sealed record CreateContactInfoDto
    {
        public string Address { get; init; } = string.Empty;
        public string Telephone { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}