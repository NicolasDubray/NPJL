namespace NPJL.Application.DTOs.Employees
{
    // DTO for updating existing contact information.
    public sealed record UpdateContactInfoDto
    {
        public string Address { get; init; } = string.Empty;
        public string Telephone { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}