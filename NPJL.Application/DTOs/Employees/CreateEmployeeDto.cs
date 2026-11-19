namespace NPJL.Application.DTOs.Employees
{
    public sealed record CreateEmployeeDto
    {
        // Personal information
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string SSN { get; init; } = string.Empty;

        // Contact information
        public string Address { get; init; } = string.Empty;
        public string Telephone { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;

        // Account information
        public string Password { get; init; } = string.Empty;

        // Role selection
        // False = Employee, True = Administrator
        public bool IsAdministrator { get; init; }
    }
}